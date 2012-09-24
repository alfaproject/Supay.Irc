using System;
using System.Collections.Generic;
using System.Globalization;

namespace Supay.Irc.Messages.Modes
{
    /// <summary>
    ///   ChannelModesCreator parses, builds, and writes the modes used by the <see cref="ChannelModeMessage" /> class.
    /// </summary>
    public class ChannelModesCreator
    {
        private readonly ChannelModeCollection _modes = new ChannelModeCollection();
        private ServerSupport _serverSupports = new ServerSupport();


        #region Parsing

        /// <summary>
        ///   Loads the given mode data into this <see cref="ChannelModesCreator" />
        /// </summary>
        public void Parse(string modeChanges, IList<string> modeArguments)
        {
            if (string.IsNullOrEmpty(modeChanges))
            {
                return;
            }
            if (modeArguments == null)
            {
                modeArguments = new List<string>();
            }

            this._modes.Clear();
            var currentAction = ModeAction.Add;
            int argIndex = 0;
            foreach (char c in modeChanges)
            {
                switch (c)
                {
                    case '+':
                        currentAction = ModeAction.Add;
                        break;
                    case '-':
                        currentAction = ModeAction.Remove;
                        break;

                        // PONDER This probably won't correctly parse incorrect mode messages, should I?
                    case 'a':
                        this._modes.Add(new AnonymousMode(currentAction));
                        break;
                    case 'b':
                        var banMode = new BanMode(currentAction, new User(modeArguments[argIndex]));
                        argIndex++;
                        this._modes.Add(banMode);
                        break;
                    case 'e':
                        var banExceptionMode = new BanExceptionMode(currentAction, new User(modeArguments[argIndex]));
                        argIndex++;
                        this._modes.Add(banExceptionMode);
                        break;
                    case 'h':
                        var halfOpMode = new HalfOpMode(currentAction, modeArguments[argIndex]);
                        argIndex++;
                        this._modes.Add(halfOpMode);
                        break;
                    case 'I':
                        var invitationExceptionMode = new InvitationExceptionMode(currentAction, new User(modeArguments[argIndex]));
                        argIndex++;
                        this._modes.Add(invitationExceptionMode);
                        break;
                    case 'i':
                        var inviteOnlyMode = new InviteOnlyMode(currentAction);
                        this._modes.Add(inviteOnlyMode);
                        break;
                    case 'k':
                        var keyMode = new KeyMode(currentAction);
                        if (currentAction == ModeAction.Add)
                        {
                            keyMode.Password = modeArguments[argIndex];
                            argIndex++;
                        }
                        this._modes.Add(keyMode);
                        break;
                    case 'l':
                        var limitMode = new LimitMode(currentAction);
                        if (currentAction == ModeAction.Add)
                        {
                            limitMode.UserLimit = Convert.ToInt32(modeArguments[argIndex], CultureInfo.InvariantCulture);
                            argIndex++;
                        }
                        this._modes.Add(limitMode);
                        break;
                    case 'm':
                        this._modes.Add(new ModeratedMode(currentAction));
                        break;
                    case 'n':
                        this._modes.Add(new NoOutsideMessagesMode(currentAction));
                        break;
                    case 'O':
                        var creatorMode = new CreatorMode(currentAction, modeArguments[argIndex]);
                        argIndex++;
                        this._modes.Add(creatorMode);
                        break;
                    case 'o':
                        var operatorMode = new OperatorMode(currentAction, modeArguments[argIndex]);
                        argIndex++;
                        this._modes.Add(operatorMode);
                        break;
                    case 'p':
                        this._modes.Add(new PrivateMode(currentAction));
                        break;
                    case 'q':
                        this._modes.Add(new QuietMode(currentAction));
                        break;
                    case 's':
                        this._modes.Add(new SecretMode(currentAction));
                        break;
                    case 'r':
                        this._modes.Add(new ServerReopMode(currentAction));
                        break;
                    case 'R':
                        this._modes.Add(new RegisteredNicksOnlyMode(currentAction));
                        break;
                    case 't':
                        this._modes.Add(new TopicGuardedMode(currentAction));
                        break;
                    case 'v':
                        var voiceMode = new VoiceMode(currentAction, modeArguments[argIndex]);
                        argIndex++;
                        this._modes.Add(voiceMode);
                        break;
                    default:
                        string unknownMode = c.ToString(CultureInfo.InvariantCulture);
                        if (this._serverSupports.ModesWithParameters.Contains(unknownMode) || (this._serverSupports.ModesWithParametersWhenSet.Contains(unknownMode) && currentAction == ModeAction.Add))
                        {
                            // I want to yank a parameter
                            this._modes.Add(new UnknownChannelMode(currentAction, unknownMode, modeArguments[argIndex]));
                            argIndex++;
                        }
                        else
                        {
                            // I don't
                            this._modes.Add(new UnknownChannelMode(currentAction, unknownMode));
                        }
                        break;
                }
            }
            this.CollapseModes();
        }

        /// <summary>
        ///   Loads the given mode data into this <see cref="ChannelModesCreator" />
        /// </summary>
        public void Parse(ChannelModeMessage msg)
        {
            if (msg == null)
            {
                return;
            }
            this.Parse(msg.ModeChanges, msg.ModeArguments);
        }

        /// <summary>
        ///   Loads the given mode data into this <see cref="ChannelModesCreator" />
        /// </summary>
        public void Parse(string modeChanges)
        {
            if (string.IsNullOrEmpty(modeChanges))
            {
                return;
            }
            this.Parse(modeChanges, new List<string>());
        }

        #endregion


        /// <summary>
        ///   A <see cref="Supay.Irc.ServerSupport" /> instance is required in order to parse non-standard modes.
        /// </summary>
        public ServerSupport ServerSupport
        {
            get
            {
                return this._serverSupports;
            }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }
                this._serverSupports = value;
            }
        }

        /// <summary>
        ///   Gets the collection of modes parsed or to be applied.
        /// </summary>
        public virtual IList<ChannelMode> Modes
        {
            get
            {
                return this._modes;
            }
        }

        /// <summary>
        ///   Removes redundant or overridden modes from the modes collection.
        /// </summary>
        private void CollapseModes()
        {
            // TODO Implement CollapseModes
        }

        /// <summary>
        ///   Applies the current modes to the given <see cref="ChannelModeMessage" />.
        /// </summary>
        /// <param name="msg">The message to be altered.</param>
        public virtual void ApplyTo(ChannelModeMessage msg)
        {
            if (msg == null)
            {
                return;
            }
            msg.ModeChanges = string.Empty;
            msg.ModeArguments.Clear();
            if (this._modes.Count > 0)
            {
                // The first one always adds its mode
                ChannelMode currentMode = this._modes[0];
                ModeAction currentAction = currentMode.Action;
                currentMode.ApplyTo(msg, true);

                // The rest compare to the current
                for (int i = 1; i < this._modes.Count; i++)
                {
                    currentMode = this._modes[i];
                    currentMode.ApplyTo(msg, currentAction != currentMode.Action);
                    currentAction = currentMode.Action;
                }
            }
        }
    }
}
