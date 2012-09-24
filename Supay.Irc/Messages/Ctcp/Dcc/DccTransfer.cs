using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using Supay.Irc.Properties;

namespace Supay.Irc.Dcc
{
    /// <summary>
    ///   Handles the networks level communication protocols for sending and receiving files over DCC.
    /// </summary>
    public class DccTransfer
    {
        private byte[] _buffer;
        private int _bufferSize = 4096;


        #region Constructor

        /// <summary>
        ///   Initializes a new instance of the DccTransfer class.
        /// </summary>
        public DccTransfer()
        {
            this.FileSize = -1;
            this.BytesTransferred = 0;
            this.SendAhead = true;
            this.Secure = false;
            this.TurboMode = false;
            this.StartPosition = 0;
        }

        #endregion


        #region Properties

        /// <summary>
        ///   Gets or sets a stream to the file being transferred.
        /// </summary>
        public FileStream File
        {
            get;
            set;
        }

        /// <summary>
        ///   Gets or sets the start position in the file to transfer the information.
        /// </summary>
        public long StartPosition
        {
            get;
            set;
        }

        /// <summary>
        ///   Gets or sets the socket the file transfer will use.
        /// </summary>
        public Socket TransferSocket
        {
            get;
            set;
        }

        /// <summary>
        ///   Gets or sets the size of the buffer for transfer of the file.
        /// </summary>
        public int BufferSize
        {
            get
            {
                return this._bufferSize;
            }
            set
            {
                if (value > 8192)
                {
                    throw new ArgumentException(Resources.BufferSizeIsLimited, "value");
                }
                this._bufferSize = value;
            }
        }

        /// <summary>
        ///   Gets or sets if the transfer uses the "turbo" extension to increase transfer speed.
        /// </summary>
        public bool TurboMode
        {
            get;
            set;
        }

        /// <summary>
        ///   Gets or sets if the transfer uses SSL to secure the transfer.
        /// </summary>
        public bool Secure
        {
            get;
            set;
        }

        /// <summary>
        ///   Gets or sets if the transfer uses the "send ahead" extension to increase transfer speed.
        /// </summary>
        public bool SendAhead
        {
            get;
            set;
        }

        /// <summary>
        ///   Gets the number of bytes transferred so far.
        /// </summary>
        public long BytesTransferred
        {
            get;
            private set;
        }

        /// <summary>
        ///   Gets or sets the size of the file being transferred.
        /// </summary>
        public long FileSize
        {
            get;
            set;
        }

        #endregion


        #region Events

        /// <summary>
        ///   The TransferInterruption event occurs when the file has not completely transferred, but
        ///   the connection has been stopped.
        /// </summary>
        public event EventHandler TransferInterruption;

        /// <summary>
        ///   Raises the <see cref="TransferInterruption" /> event.
        /// </summary>
        protected void OnTransferInterruption(EventArgs e)
        {
            if (this.TransferInterruption != null)
            {
                this.TransferInterruption(this, e);
            }
        }

        /// <summary>
        ///   The TransferComplete event occurs when the file has been completely transferred.
        /// </summary>
        public event EventHandler TransferComplete;

        /// <summary>
        ///   Raises the <see cref="TransferComplete" /> event.
        /// </summary>
        protected void OnTransferComplete(EventArgs e)
        {
            if (this.TransferComplete != null)
            {
                this.TransferComplete(this, e);
            }
        }

        #endregion


        #region Internal Methods

        /// <summary>
        ///   Sends the file over the current socket.
        /// </summary>
        internal void Send()
        {
            if (!this.File.CanRead)
            {
                throw new InvalidOperationException(Resources.CannotReadFromFile);
            }

            this.BytesTransferred = 0;

            this._buffer = new byte[this.BufferSize];
            var acknowledgment = new byte[4];

            int bytesSent;
            while ((bytesSent = this.File.Read(this._buffer, 0, this._buffer.Length)) != 0)
            {
                try
                {
                    this.TransferSocket.Send(this._buffer, bytesSent, SocketFlags.None);
                    this.BytesTransferred += bytesSent;
                    if (!this.TurboMode && !this.SendAhead)
                    {
                        this.TransferSocket.Receive(acknowledgment);
                    }
                }
                catch
                {
                    this.OnTransferInterruption(EventArgs.Empty);
                }
            }

            if (!this.TurboMode)
            {
                while (!this.AllAcknowledgementsReceived(acknowledgment))
                {
                    this.TransferSocket.Receive(acknowledgment);
                }
            }
            this.OnTransferComplete(EventArgs.Empty);
        }

        /// <summary>
        ///   Receives the file over the current socket.
        /// </summary>
        internal void Receive()
        {
            this.BytesTransferred = 0;

            this._buffer = new byte[this.BufferSize];

            while (!this.IsTransferComplete)
            {
                int bytesReceived = this.TransferSocket.Receive(this._buffer);
                if (bytesReceived == 0)
                {
                    this.OnTransferInterruption(EventArgs.Empty);
                    return;
                }
                this.BytesTransferred += bytesReceived;
                if (this.File.CanWrite)
                {
                    this.File.Write(this._buffer, 0, bytesReceived);
                }
                this.SendAcknowledgement();
            }
            this.File.Flush();
            this.OnTransferComplete(EventArgs.Empty);
        }

        #endregion


        #region Private Methods

        private bool IsTransferComplete
        {
            get
            {
                if (this.FileSize == -1)
                {
                    return false;
                }
                return this.StartPosition + this.BytesTransferred >= this.FileSize;
            }
        }

        private void SendAcknowledgement()
        {
            if (this.TurboMode)
            {
                return;
            }

            // convert BytesTransfered to a 4 byte array containing the number
            var bytesAck = this.DccBytesReceivedFormat();

            // send it over the socket
            this.TransferSocket.Send(bytesAck);
        }

        private bool AllAcknowledgementsReceived(byte[] lastAck)
        {
            long acknowledgedBytes = DccBytesToLong(lastAck);
            return acknowledgedBytes >= this.BytesTransferred;
        }

        private byte[] DccBytesReceivedFormat()
        {
            var size = new byte[4];
            var longBytes = BitConverter.GetBytes(NetworkUnsignedLong(this.BytesTransferred));
            Array.Copy(longBytes, 0, size, 0, 4);
            return size;
        }

        private static long DccBytesToLong(byte[] received)
        {
            return IPAddress.NetworkToHostOrder(BitConverter.ToInt32(received, 0));
        }

        private static long NetworkUnsignedLong(long hostOrderLong)
        {
            long networkLong = IPAddress.HostToNetworkOrder(hostOrderLong);
            return (networkLong >> 32) & 0x00000000ffffffff;
        }

        #endregion
    }
}
