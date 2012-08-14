namespace Supay.Irc.Messages
{
    /// <summary>
    ///   The base class for classes which send and receive messages.
    /// </summary>
    public class MessageConduit
    {
        /// <summary>
        ///   Occurs when an unrecognised message is received.
        /// </summary>
        public event IrcMessageEventHandler<GenericMessage> GenericMessage;

        /// <summary>
        ///   Raises the GenericMessage event.
        /// </summary>
        protected internal void OnGenericMessage(IrcMessageEventArgs<GenericMessage> e)
        {
            if (this.GenericMessage != null)
            {
                this.GenericMessage(this, e);
            }
        }


        #region Errors

        /// <summary>
        ///   Occurs when an unrecognised error message is received.
        /// </summary>
        public event IrcMessageEventHandler<GenericErrorMessage> GenericErrorMessage;

        /// <summary>
        ///   Raises the GenericErrorMessage event.
        /// </summary>
        protected internal void OnGenericErrorMessage(IrcMessageEventArgs<GenericErrorMessage> e)
        {
            if (this.GenericErrorMessage != null)
            {
                this.GenericErrorMessage(this, e);
            }
        }

        /// <summary>
        ///   Occurs when a <see cref="ErroneusNickMessage" /> is received.
        /// </summary>
        public event IrcMessageEventHandler<ErroneusNickMessage> ErroneusNick;

        /// <summary>
        ///   Raises the ErroneusNick event.
        /// </summary>
        protected internal void OnErroneusNick(IrcMessageEventArgs<ErroneusNickMessage> e)
        {
            if (this.ErroneusNick != null)
            {
                this.ErroneusNick(this, e);
            }
        }

        /// <summary>
        ///   Occurs when a <see cref="NickCollisionMessage" /> is received.
        /// </summary>
        public event IrcMessageEventHandler<NickCollisionMessage> NickCollision;

        /// <summary>
        ///   Raises the NickCollision event.
        /// </summary>
        protected internal void OnNickCollision(IrcMessageEventArgs<NickCollisionMessage> e)
        {
            if (this.NickCollision != null)
            {
                this.NickCollision(this, e);
            }
        }

        /// <summary>
        ///   Occurs when a <see cref="NickInUseMessage" /> is received.
        /// </summary>
        public event IrcMessageEventHandler<NickInUseMessage> NickInUse;

        /// <summary>
        ///   Raises the NickCollision event.
        /// </summary>
        protected internal void OnNickInUse(IrcMessageEventArgs<NickInUseMessage> e)
        {
            if (this.NickInUse != null)
            {
                this.NickInUse(this, e);
            }
        }

        /// <summary>
        ///   Occurs when a <see cref="NoHostPermissionMessage" /> is received.
        /// </summary>
        public event IrcMessageEventHandler<NoHostPermissionMessage> NoHostPermission;

        /// <summary>
        ///   Raises the NoHostPermission event.
        /// </summary>
        protected internal void OnNoHostPermission(IrcMessageEventArgs<NoHostPermissionMessage> e)
        {
            if (this.NoHostPermission != null)
            {
                this.NoHostPermission(this, e);
            }
        }

        /// <summary>
        ///   Occurs when a <see cref="NoNickGivenMessage" /> is received.
        /// </summary>
        public event IrcMessageEventHandler<NoNickGivenMessage> NoNickGiven;

        /// <summary>
        ///   Raises the NoNickGiven event.
        /// </summary>
        protected internal void OnNoNickGiven(IrcMessageEventArgs<NoNickGivenMessage> e)
        {
            if (this.NoNickGiven != null)
            {
                this.NoNickGiven(this, e);
            }
        }

        /// <summary>
        ///   Occurs when a <see cref="NotRegisteredMessage" /> is received.
        /// </summary>
        public event IrcMessageEventHandler<NotRegisteredMessage> NotRegistered;

        /// <summary>
        ///   Raises the NotRegistered event.
        /// </summary>
        protected internal void OnNotRegistered(IrcMessageEventArgs<NotRegisteredMessage> e)
        {
            if (this.NotRegistered != null)
            {
                this.NotRegistered(this, e);
            }
        }

        /// <summary>
        ///   Occurs when a <see cref="YouAreBannedMessage" /> is received.
        /// </summary>
        public event IrcMessageEventHandler<YouAreBannedMessage> YouAreBanned;

        /// <summary>
        ///   Raises the YouAreBanned event.
        /// </summary>
        protected internal void OnYouAreBanned(IrcMessageEventArgs<YouAreBannedMessage> e)
        {
            if (this.YouAreBanned != null)
            {
                this.YouAreBanned(this, e);
            }
        }

        /// <summary>
        ///   Occurs when a <see cref="NoSuchChannelMessage" /> is received.
        /// </summary>
        public event IrcMessageEventHandler<NoSuchChannelMessage> NoSuchChannel;

        /// <summary>
        ///   Raises the NoSuchChannel event.
        /// </summary>
        protected internal void OnNoSuchChannel(IrcMessageEventArgs<NoSuchChannelMessage> e)
        {
            if (this.NoSuchChannel != null)
            {
                this.NoSuchChannel(this, e);
            }
        }

        /// <summary>
        ///   Occurs when a <see cref="NoSuchNickMessage" /> is received.
        /// </summary>
        public event IrcMessageEventHandler<NoSuchNickMessage> NoSuchNick;

        /// <summary>
        ///   Raises the NoSuchNick event.
        /// </summary>
        protected internal void OnNoSuchNick(IrcMessageEventArgs<NoSuchNickMessage> e)
        {
            if (this.NoSuchNick != null)
            {
                this.NoSuchNick(this, e);
            }
        }

        /// <summary>
        ///   Occurs when a <see cref="NoSuchServerMessage" /> is received.
        /// </summary>
        public event IrcMessageEventHandler<NoSuchServerMessage> NoSuchServer;

        /// <summary>
        ///   Raises the NoSuchServer event.
        /// </summary>
        protected internal void OnNoSuchServer(IrcMessageEventArgs<NoSuchServerMessage> e)
        {
            if (this.NoSuchServer != null)
            {
                this.NoSuchServer(this, e);
            }
        }

        /// <summary>
        ///   Occurs when a <see cref="CannotSendToChannelMessage" /> is received.
        /// </summary>
        public event IrcMessageEventHandler<CannotSendToChannelMessage> CannotSendToChannel;

        /// <summary>
        ///   Raises the <see cref="CannotSendToChannel" /> event.
        /// </summary>
        protected internal void OnCannotSendToChannel(IrcMessageEventArgs<CannotSendToChannelMessage> e)
        {
            if (this.CannotSendToChannel != null)
            {
                this.CannotSendToChannel(this, e);
            }
        }

        /// <summary>
        ///   Occurs when a <see cref="TooManyChannelsMessage" /> is received.
        /// </summary>
        public event IrcMessageEventHandler<TooManyChannelsMessage> TooManyChannels;

        /// <summary>
        ///   Raises the <see cref="TooManyChannels" /> event.
        /// </summary>
        protected internal void OnTooManyChannels(IrcMessageEventArgs<TooManyChannelsMessage> e)
        {
            if (this.TooManyChannels != null)
            {
                this.TooManyChannels(this, e);
            }
        }

        /// <summary>
        ///   Occurs when a <see cref="TooManyChannelsMessage" /> is received.
        /// </summary>
        public event IrcMessageEventHandler<TooManyTargetsMessage> TooManyTargets;

        /// <summary>
        ///   Raises the <see cref="TooManyTargets" /> event.
        /// </summary>
        protected internal void OnTooManyTargets(IrcMessageEventArgs<TooManyTargetsMessage> e)
        {
            if (this.TooManyTargets != null)
            {
                this.TooManyTargets(this, e);
            }
        }

        /// <summary>
        ///   Occurs when a <see cref="WasNoSuchNickMessage" /> is received.
        /// </summary>
        public event IrcMessageEventHandler<WasNoSuchNickMessage> WasNoSuchNick;

        /// <summary>
        ///   Raises the <see cref="WasNoSuchNick" /> event.
        /// </summary>
        protected internal void OnWasNoSuchNick(IrcMessageEventArgs<WasNoSuchNickMessage> e)
        {
            if (this.WasNoSuchNick != null)
            {
                this.WasNoSuchNick(this, e);
            }
        }

        /// <summary>
        ///   Occurs when a <see cref="CannotUseColorsMessage" /> is received.
        /// </summary>
        public event IrcMessageEventHandler<CannotUseColorsMessage> CannotUseColors;

        /// <summary>
        ///   Raises the <see cref="CannotUseColors" /> event.
        /// </summary>
        protected internal void OnCannotUseColors(IrcMessageEventArgs<CannotUseColorsMessage> ircMessageEventArgs)
        {
            if (this.CannotUseColors != null)
            {
                this.CannotUseColors(this, ircMessageEventArgs);
            }
        }

        /// <summary>
        ///   Occurs when a <see cref="NoPingOriginSpecifiedMessage" /> is received.
        /// </summary>
        public event IrcMessageEventHandler<NoPingOriginSpecifiedMessage> NoPingOriginSpecified;

        /// <summary>
        ///   Raises the <see cref="NoPingOriginSpecified" /> event.
        /// </summary>
        protected internal void OnNoPingOriginSpecified(IrcMessageEventArgs<NoPingOriginSpecifiedMessage> ircMessageEventArgs)
        {
            if (this.NoPingOriginSpecified != null)
            {
                this.NoPingOriginSpecified(this, ircMessageEventArgs);
            }
        }

        /// <summary>
        ///   Occurs when a <see cref="NoRecipientGivenMessage" /> is received.
        /// </summary>
        public event IrcMessageEventHandler<NoRecipientGivenMessage> NoRecipientGiven;

        /// <summary>
        ///   Raises the <see cref="NoRecipientGiven" /> event.
        /// </summary>
        protected internal void OnNoRecipientGiven(IrcMessageEventArgs<NoRecipientGivenMessage> ircMessageEventArgs)
        {
            if (this.NoRecipientGiven != null)
            {
                this.NoRecipientGiven(this, ircMessageEventArgs);
            }
        }

        /// <summary>
        ///   Occurs when a <see cref="NoTextToSendMessage" /> is received.
        /// </summary>
        public event IrcMessageEventHandler<NoTextToSendMessage> NoTextToSend;

        /// <summary>
        ///   Raises the <see cref="NoTextToSend" /> event.
        /// </summary>
        protected internal void OnNoTextToSend(IrcMessageEventArgs<NoTextToSendMessage> ircMessageEventArgs)
        {
            if (this.NoTextToSend != null)
            {
                this.NoTextToSend(this, ircMessageEventArgs);
            }
        }

        /// <summary>
        ///   Occurs when a <see cref="TooManyLinesMessage" /> is received.
        /// </summary>
        public event IrcMessageEventHandler<TooManyLinesMessage> TooManyLines;

        /// <summary>
        ///   Raises the <see cref="NoTextToSend" /> event.
        /// </summary>
        protected internal void OnTooManyLines(IrcMessageEventArgs<TooManyLinesMessage> ircMessageEventArgs)
        {
            if (this.TooManyLines != null)
            {
                this.TooManyLines(this, ircMessageEventArgs);
            }
        }

        /// <summary>
        ///   Occurs when a <see cref="UnknownCommandMessage" /> is received.
        /// </summary>
        public event IrcMessageEventHandler<UnknownCommandMessage> UnknownCommand;

        /// <summary>
        ///   Raises the <see cref="UnknownCommand" /> event.
        /// </summary>
        protected internal void OnUnknownCommand(IrcMessageEventArgs<UnknownCommandMessage> ircMessageEventArgs)
        {
            if (this.UnknownCommand != null)
            {
                this.UnknownCommand(this, ircMessageEventArgs);
            }
        }

        /// <summary>
        ///   Occurs when a <see cref="CannotChangeNickWhileBannedMessage" /> is received.
        /// </summary>
        public event IrcMessageEventHandler<CannotChangeNickWhileBannedMessage> CannotChangeNickWhileBanned;

        /// <summary>
        ///   Raises the <see cref="CannotChangeNickWhileBanned" /> event.
        /// </summary>
        protected internal void OnCannotChangeNickWhileBanned(IrcMessageEventArgs<CannotChangeNickWhileBannedMessage> ircMessageEventArgs)
        {
            if (this.CannotChangeNickWhileBanned != null)
            {
                this.CannotChangeNickWhileBanned(this, ircMessageEventArgs);
            }
        }

        /// <summary>
        ///   Occurs when a <see cref="NickChangeTooFastMessage" /> is received.
        /// </summary>
        public event IrcMessageEventHandler<NickChangeTooFastMessage> NickChangeTooFast;

        /// <summary>
        ///   Raises the <see cref="NickChangeTooFast" /> event.
        /// </summary>
        protected internal void OnNickChangeTooFast(IrcMessageEventArgs<NickChangeTooFastMessage> ircMessageEventArgs)
        {
            if (this.NickChangeTooFast != null)
            {
                this.NickChangeTooFast(this, ircMessageEventArgs);
            }
        }

        /// <summary>
        ///   Occurs when a <see cref="TargetChangeTooFastMessage" /> is received.
        /// </summary>
        public event IrcMessageEventHandler<TargetChangeTooFastMessage> TargetChangeTooFast;

        /// <summary>
        ///   Raises the <see cref="TargetChangeTooFast" /> event.
        /// </summary>
        protected internal void OnTargetChangeTooFast(IrcMessageEventArgs<TargetChangeTooFastMessage> ircMessageEventArgs)
        {
            if (this.TargetChangeTooFast != null)
            {
                this.TargetChangeTooFast(this, ircMessageEventArgs);
            }
        }

        /// <summary>
        ///   Occurs when a <see cref="NotOnChannelMessage" /> is received.
        /// </summary>
        public event IrcMessageEventHandler<NotOnChannelMessage> NotOnChannel;

        /// <summary>
        ///   Raises the <see cref="NotOnChannel" /> event.
        /// </summary>
        protected internal void OnNotOnChannel(IrcMessageEventArgs<NotOnChannelMessage> ircMessageEventArgs)
        {
            if (this.NotOnChannel != null)
            {
                this.NotOnChannel(this, ircMessageEventArgs);
            }
        }

        /// <summary>
        ///   Occurs when a <see cref="AlreadyOnChannelMessage" /> is received.
        /// </summary>
        public event IrcMessageEventHandler<AlreadyOnChannelMessage> AlreadyOnChannel;

        /// <summary>
        ///   Raises the <see cref="AlreadyOnChannel" /> event.
        /// </summary>
        protected internal void OnAlreadyOnChannel(IrcMessageEventArgs<AlreadyOnChannelMessage> ircMessageEventArgs)
        {
            if (this.AlreadyOnChannel != null)
            {
                this.AlreadyOnChannel(this, ircMessageEventArgs);
            }
        }

        /// <summary>
        ///   Occurs when a <see cref="IdentChangedMessage" /> is received.
        /// </summary>
        public event IrcMessageEventHandler<IdentChangedMessage> IdentChanged;

        /// <summary>
        ///   Raises the <see cref="IdentChanged" /> event.
        /// </summary>
        protected internal void OnIdentChanged(IrcMessageEventArgs<IdentChangedMessage> ircMessageEventArgs)
        {
            if (this.IdentChanged != null)
            {
                this.IdentChanged(this, ircMessageEventArgs);
            }
        }

        /// <summary>
        ///   Occurs when a <see cref="NotEnoughParametersMessage" /> is received.
        /// </summary>
        public event IrcMessageEventHandler<NotEnoughParametersMessage> NotEnoughParameters;

        /// <summary>
        ///   Raises the <see cref="NotEnoughParameters" /> event.
        /// </summary>
        protected internal void OnNotEnoughParameters(IrcMessageEventArgs<NotEnoughParametersMessage> ircMessageEventArgs)
        {
            if (this.NotEnoughParameters != null)
            {
                this.NotEnoughParameters(this, ircMessageEventArgs);
            }
        }

        /// <summary>
        ///   Occurs when a <see cref="ChannelKeyAlreadySetMessage" /> is received.
        /// </summary>
        public event IrcMessageEventHandler<ChannelKeyAlreadySetMessage> ChannelKeyAlreadySet;

        /// <summary>
        ///   Raises the <see cref="ChannelKeyAlreadySet" /> event.
        /// </summary>
        protected internal void OnChannelKeyAlreadySet(IrcMessageEventArgs<ChannelKeyAlreadySetMessage> ircMessageEventArgs)
        {
            if (this.ChannelKeyAlreadySet != null)
            {
                this.ChannelKeyAlreadySet(this, ircMessageEventArgs);
            }
        }

        /// <summary>
        ///   Occurs when a <see cref="ChannelLimitReachedMessage" /> is received.
        /// </summary>
        public event IrcMessageEventHandler<ChannelLimitReachedMessage> ChannelLimitReached;

        /// <summary>
        ///   Raises the <see cref="ChannelLimitReached" /> event.
        /// </summary>
        protected internal void OnChannelLimitReached(IrcMessageEventArgs<ChannelLimitReachedMessage> ircMessageEventArgs)
        {
            if (this.ChannelLimitReached != null)
            {
                this.ChannelLimitReached(this, ircMessageEventArgs);
            }
        }

        /// <summary>
        ///   Occurs when a <see cref="UnknownChannelModeMessage" /> is received.
        /// </summary>
        public event IrcMessageEventHandler<UnknownChannelModeMessage> UnknownChannelMode;

        /// <summary>
        ///   Raises the <see cref="UnknownChannelMode" /> event.
        /// </summary>
        protected internal void OnUnknownChannelMode(IrcMessageEventArgs<UnknownChannelModeMessage> ircMessageEventArgs)
        {
            if (this.UnknownChannelMode != null)
            {
                this.UnknownChannelMode(this, ircMessageEventArgs);
            }
        }

        /// <summary>
        ///   Occurs when a <see cref="ChannelIsInviteOnlyMessage" /> is received.
        /// </summary>
        public event IrcMessageEventHandler<ChannelIsInviteOnlyMessage> ChannelIsInviteOnly;

        /// <summary>
        ///   Raises the <see cref="ChannelIsInviteOnly" /> event.
        /// </summary>
        protected internal void OnChannelIsInviteOnly(IrcMessageEventArgs<ChannelIsInviteOnlyMessage> ircMessageEventArgs)
        {
            if (this.ChannelIsInviteOnly != null)
            {
                this.ChannelIsInviteOnly(this, ircMessageEventArgs);
            }
        }

        /// <summary>
        ///   Occurs when a <see cref="YouAreBannedFromChannelMessage" /> is received.
        /// </summary>
        public event IrcMessageEventHandler<YouAreBannedFromChannelMessage> YouAreBannedFromChannel;

        /// <summary>
        ///   Raises the <see cref="YouAreBannedFromChannel" /> event.
        /// </summary>
        protected internal void OnYouAreBannedFromChannel(IrcMessageEventArgs<YouAreBannedFromChannelMessage> ircMessageEventArgs)
        {
            if (this.YouAreBannedFromChannel != null)
            {
                this.YouAreBannedFromChannel(this, ircMessageEventArgs);
            }
        }

        /// <summary>
        ///   Occurs when a <see cref="ChannelRequiresKeyMessage" /> is received.
        /// </summary>
        public event IrcMessageEventHandler<ChannelRequiresKeyMessage> ChannelRequiresKey;

        /// <summary>
        ///   Raises the <see cref="ChannelRequiresKey" /> event.
        /// </summary>
        protected internal void OnChannelRequiresKey(IrcMessageEventArgs<ChannelRequiresKeyMessage> ircMessageEventArgs)
        {
            if (this.ChannelRequiresKey != null)
            {
                this.ChannelRequiresKey(this, ircMessageEventArgs);
            }
        }

        /// <summary>
        ///   Occurs when a <see cref="ChannelRequiresRegisteredNickMessage" /> is received.
        /// </summary>
        public event IrcMessageEventHandler<ChannelRequiresRegisteredNickMessage> ChannelRequiresRegisteredNick;

        /// <summary>
        ///   Raises the <see cref="ChannelRequiresRegisteredNick" /> event.
        /// </summary>
        protected internal void OnChannelRequiresRegisteredNick(IrcMessageEventArgs<ChannelRequiresRegisteredNickMessage> ircMessageEventArgs)
        {
            if (this.ChannelRequiresRegisteredNick != null)
            {
                this.ChannelRequiresRegisteredNick(this, ircMessageEventArgs);
            }
        }

        /// <summary>
        ///   Occurs when a <see cref="BanListFullMessage" /> is received.
        /// </summary>
        public event IrcMessageEventHandler<BanListFullMessage> BanListFull;

        /// <summary>
        ///   Raises the <see cref="BanListFull" /> event.
        /// </summary>
        protected internal void OnBanListFull(IrcMessageEventArgs<BanListFullMessage> ircMessageEventArgs)
        {
            if (this.BanListFull != null)
            {
                this.BanListFull(this, ircMessageEventArgs);
            }
        }

        /// <summary>
        ///   Occurs when a <see cref="ChannelOperatorStatusRequiredMessage" /> is received.
        /// </summary>
        public event IrcMessageEventHandler<ChannelOperatorStatusRequiredMessage> ChannelOperatorStatusRequired;

        /// <summary>
        ///   Raises the <see cref="ChannelOperatorStatusRequired" /> event.
        /// </summary>
        protected internal void OnChannelOperatorStatusRequired(IrcMessageEventArgs<ChannelOperatorStatusRequiredMessage> ircMessageEventArgs)
        {
            if (this.ChannelOperatorStatusRequired != null)
            {
                this.ChannelOperatorStatusRequired(this, ircMessageEventArgs);
            }
        }

        /// <summary>
        ///   Occurs when a <see cref="CannotRemoveServiceBotMessage" /> is received.
        /// </summary>
        public event IrcMessageEventHandler<CannotRemoveServiceBotMessage> CannotRemoveServiceBot;

        /// <summary>
        ///   Raises the <see cref="CannotRemoveServiceBot" /> event.
        /// </summary>
        protected internal void OnCannotRemoveServiceBot(IrcMessageEventArgs<CannotRemoveServiceBotMessage> ircMessageEventArgs)
        {
            if (this.CannotRemoveServiceBot != null)
            {
                this.CannotRemoveServiceBot(this, ircMessageEventArgs);
            }
        }

        /// <summary>
        ///   Occurs when a <see cref="ChannelBlockedMessage" /> is received.
        /// </summary>
        public event IrcMessageEventHandler<ChannelBlockedMessage> ChannelBlocked;

        /// <summary>
        ///   Raises the <see cref="ChannelBlocked" /> event.
        /// </summary>
        protected internal void OnChannelBlocked(IrcMessageEventArgs<ChannelBlockedMessage> ircMessageEventArgs)
        {
            if (this.ChannelBlocked != null)
            {
                this.ChannelBlocked(this, ircMessageEventArgs);
            }
        }

        /// <summary>
        ///   Occurs when a <see cref="UnknownUserModeMessage" /> is received.
        /// </summary>
        public event IrcMessageEventHandler<UnknownUserModeMessage> UnknownUserMode;

        /// <summary>
        ///   Raises the <see cref="UnknownUserMode" /> event.
        /// </summary>
        protected internal void OnUnknownUserMode(IrcMessageEventArgs<UnknownUserModeMessage> ircMessageEventArgs)
        {
            if (this.UnknownUserMode != null)
            {
                this.UnknownUserMode(this, ircMessageEventArgs);
            }
        }

        /// <summary>
        ///   Occurs when a <see cref="SilenceListFullMessage" /> is received.
        /// </summary>
        public event IrcMessageEventHandler<SilenceListFullMessage> SilenceListFull;

        /// <summary>
        ///   Raises the <see cref="SilenceListFull" /> event.
        /// </summary>
        protected internal void OnSilenceListFull(IrcMessageEventArgs<SilenceListFullMessage> ircMessageEventArgs)
        {
            if (this.SilenceListFull != null)
            {
                this.SilenceListFull(this, ircMessageEventArgs);
            }
        }

        /// <summary>
        ///   Occurs when a <see cref="AcceptListFullMessage" /> is received.
        /// </summary>
        public event IrcMessageEventHandler<AcceptListFullMessage> AcceptListFull;

        /// <summary>
        ///   Raises the <see cref="AcceptListFull" /> event.
        /// </summary>
        protected internal void OnAcceptListFull(IrcMessageEventArgs<AcceptListFullMessage> ircMessageEventArgs)
        {
            if (this.AcceptListFull != null)
            {
                this.AcceptListFull(this, ircMessageEventArgs);
            }
        }

        /// <summary>
        ///   Occurs when a <see cref="AcceptAlreadyExistsMessage" /> is received.
        /// </summary>
        public event IrcMessageEventHandler<AcceptAlreadyExistsMessage> AcceptAlreadyExists;

        /// <summary>
        ///   Raises the <see cref="AcceptAlreadyExists" /> event.
        /// </summary>
        protected internal void OnAcceptAlreadyExists(IrcMessageEventArgs<AcceptAlreadyExistsMessage> ircMessageEventArgs)
        {
            if (this.AcceptAlreadyExists != null)
            {
                this.AcceptAlreadyExists(this, ircMessageEventArgs);
            }
        }

        /// <summary>
        ///   Occurs when a <see cref="AcceptDoesNotExistMessage" /> is received.
        /// </summary>
        public event IrcMessageEventHandler<AcceptDoesNotExistMessage> AcceptDoesNotExist;

        /// <summary>
        ///   Raises the <see cref="AcceptDoesNotExist" /> event.
        /// </summary>
        protected internal void OnAcceptDoesNotExist(IrcMessageEventArgs<AcceptDoesNotExistMessage> ircMessageEventArgs)
        {
            if (this.AcceptDoesNotExist != null)
            {
                this.AcceptDoesNotExist(this, ircMessageEventArgs);
            }
        }

        #endregion


        #region Commands

        /// <summary>
        ///   Occurs when a <see cref="PingMessage" /> is received.
        /// </summary>
        public event IrcMessageEventHandler<PingMessage> Ping;

        /// <summary>
        ///   Raises the Ping event.
        /// </summary>
        protected internal void OnPing(IrcMessageEventArgs<PingMessage> e)
        {
            if (this.Ping != null)
            {
                this.Ping(this, e);
            }
        }

        /// <summary>
        ///   Occurs when a <see cref="PongMessage" /> is received.
        /// </summary>
        public event IrcMessageEventHandler<PongMessage> Pong;

        /// <summary>
        ///   Raises the Pong event.
        /// </summary>
        protected internal void OnPong(IrcMessageEventArgs<PongMessage> e)
        {
            if (this.Pong != null)
            {
                this.Pong(this, e);
            }
        }

        /// <summary>
        ///   Occurs when a <see cref="ChatMessage" /> is received.
        /// </summary>
        public event IrcMessageEventHandler<TextMessage> Chat;

        /// <summary>
        ///   Raises the Chat event.
        /// </summary>
        protected internal void OnChat(IrcMessageEventArgs<TextMessage> e)
        {
            if (this.Chat != null)
            {
                this.Chat(this, e);
            }
        }

        /// <summary>
        ///   Occurs when a <see cref="NoticeMessage" /> is received.
        /// </summary>
        public event IrcMessageEventHandler<TextMessage> Notice;

        /// <summary>
        ///   Raises the Notice event.
        /// </summary>
        protected internal void OnNotice(IrcMessageEventArgs<TextMessage> e)
        {
            if (this.Notice != null)
            {
                this.Notice(this, e);
            }
        }

        /// <summary>
        ///   Occurs when a <see cref="NickMessage" /> is received.
        /// </summary>
        public event IrcMessageEventHandler<NickMessage> NickChange;

        /// <summary>
        ///   Raises the NickChange event.
        /// </summary>
        protected internal void OnNickChange(IrcMessageEventArgs<NickMessage> e)
        {
            if (this.NickChange != null)
            {
                this.NickChange(this, e);
            }
        }

        /// <summary>
        ///   Occurs when a <see cref="JoinMessage" /> is received.
        /// </summary>
        public event IrcMessageEventHandler<JoinMessage> Join;

        /// <summary>
        ///   Raises the Join event.
        /// </summary>
        protected internal void OnJoin(IrcMessageEventArgs<JoinMessage> e)
        {
            if (this.Join != null)
            {
                this.Join(this, e);
            }
        }

        /// <summary>
        ///   Occurs when a <see cref="PartMessage" /> is received.
        /// </summary>
        public event IrcMessageEventHandler<PartMessage> Part;

        /// <summary>
        ///   Raises the Part event.
        /// </summary>
        protected internal void OnPart(IrcMessageEventArgs<PartMessage> e)
        {
            if (this.Part != null)
            {
                this.Part(this, e);
            }
        }

        /// <summary>
        ///   Occurs when a <see cref="QuitMessage" /> is received.
        /// </summary>
        public event IrcMessageEventHandler<QuitMessage> Quit;

        /// <summary>
        ///   Raises the Quit event.
        /// </summary>
        protected internal void OnQuit(IrcMessageEventArgs<QuitMessage> e)
        {
            if (this.Quit != null)
            {
                this.Quit(this, e);
            }
        }

        /// <summary>
        ///   Occurs when a <see cref="KickMessage" /> is received.
        /// </summary>
        public event IrcMessageEventHandler<KickMessage> Kick;

        /// <summary>
        ///   Raises the Kick event.
        /// </summary>
        protected internal void OnKick(IrcMessageEventArgs<KickMessage> e)
        {
            if (this.Kick != null)
            {
                this.Kick(this, e);
            }
        }

        /// <summary>
        ///   Occurs when a <see cref="TopicMessage" /> is received.
        /// </summary>
        public event IrcMessageEventHandler<TopicMessage> Topic;

        /// <summary>
        ///   Raises the Topic event.
        /// </summary>
        protected internal void OnTopic(IrcMessageEventArgs<TopicMessage> e)
        {
            if (this.Topic != null)
            {
                this.Topic(this, e);
            }
        }

        /// <summary>
        ///   Occurs when a <see cref="TopicReplyMessage" /> is received.
        /// </summary>
        public event IrcMessageEventHandler<TopicReplyMessage> TopicReply;

        /// <summary>
        ///   Raises the TopicReply event.
        /// </summary>
        protected internal void OnTopicReply(IrcMessageEventArgs<TopicReplyMessage> e)
        {
            if (this.TopicReply != null)
            {
                this.TopicReply(this, e);
            }
        }

        /// <summary>
        ///   Occurs when a <see cref="TopicNoneReplyMessage" /> is received.
        /// </summary>
        public event IrcMessageEventHandler<TopicNoneReplyMessage> TopicNoneReply;

        /// <summary>
        ///   Raises the TopicNoneReply event.
        /// </summary>
        protected internal void OnTopicNoneReply(IrcMessageEventArgs<TopicNoneReplyMessage> e)
        {
            if (this.TopicNoneReply != null)
            {
                this.TopicNoneReply(this, e);
            }
        }

        /// <summary>
        ///   Occurs when an <see cref="InviteMessage" /> is received.
        /// </summary>
        public event IrcMessageEventHandler<InviteMessage> Invite;

        /// <summary>
        ///   Raises the Invite event.
        /// </summary>
        protected internal void OnInvite(IrcMessageEventArgs<InviteMessage> e)
        {
            if (this.Invite != null)
            {
                this.Invite(this, e);
            }
        }

        /// <summary>
        ///   Occurs when a <see cref="AwayMessage" /> is received.
        /// </summary>
        public event IrcMessageEventHandler<AwayMessage> Away;

        /// <summary>
        ///   Raises the Away event.
        /// </summary>
        protected internal void OnAway(IrcMessageEventArgs<AwayMessage> e)
        {
            if (this.Away != null)
            {
                this.Away(this, e);
            }
        }

        /// <summary>
        ///   Occurs when a <see cref="AdminMessage" /> is received.
        /// </summary>
        public event IrcMessageEventHandler<AdminMessage> Admin;

        /// <summary>
        ///   Raises the Admin event.
        /// </summary>
        protected internal void OnAdmin(IrcMessageEventArgs<AdminMessage> e)
        {
            if (this.Admin != null)
            {
                this.Admin(this, e);
            }
        }

        /// <summary>
        ///   Occurs when a <see cref="BackMessage" /> is received.
        /// </summary>
        public event IrcMessageEventHandler<BackMessage> Back;

        /// <summary>
        ///   Raises the Back event.
        /// </summary>
        protected internal void OnBack(IrcMessageEventArgs<BackMessage> e)
        {
            if (this.Back != null)
            {
                this.Back(this, e);
            }
        }

        /// <summary>
        ///   Occurs when a <see cref="GenericNumericMessage" /> is received.
        /// </summary>
        public event IrcMessageEventHandler<GenericNumericMessage> GenericNumericMessage;

        /// <summary>
        ///   Raises the GenericNumericMessage event.
        /// </summary>
        protected internal void OnGenericNumericMessage(IrcMessageEventArgs<GenericNumericMessage> e)
        {
            if (this.GenericNumericMessage != null)
            {
                this.GenericNumericMessage(this, e);
            }
        }

        /// <summary>
        ///   Occurs when a <see cref="InfoMessage" /> is received.
        /// </summary>
        public event IrcMessageEventHandler<InfoMessage> Info;

        /// <summary>
        ///   Raises the Info event.
        /// </summary>
        protected internal void OnInfo(IrcMessageEventArgs<InfoMessage> e)
        {
            if (this.Info != null)
            {
                this.Info(this, e);
            }
        }

        /// <summary>
        ///   Occurs when a <see cref="IsOnMessage" /> is received.
        /// </summary>
        public event IrcMessageEventHandler<IsOnMessage> IsOn;

        /// <summary>
        ///   Raises the IsOn event.
        /// </summary>
        protected internal void OnIsOn(IrcMessageEventArgs<IsOnMessage> e)
        {
            if (this.IsOn != null)
            {
                this.IsOn(this, e);
            }
        }

        /// <summary>
        ///   Occurs when a <see cref="IsOnReplyMessage" /> is received.
        /// </summary>
        public event IrcMessageEventHandler<IsOnReplyMessage> IsOnReply;

        /// <summary>
        ///   Raises the IsOnReply event.
        /// </summary>
        protected internal void OnIsOnReply(IrcMessageEventArgs<IsOnReplyMessage> e)
        {
            if (this.IsOnReply != null)
            {
                this.IsOnReply(this, e);
            }
        }

        /// <summary>
        ///   Occurs when a <see cref="KillMessage" /> is received.
        /// </summary>
        public event IrcMessageEventHandler<KillMessage> Kill;

        /// <summary>
        ///   Raises the Kill event.
        /// </summary>
        protected internal void OnKill(IrcMessageEventArgs<KillMessage> e)
        {
            if (this.Kill != null)
            {
                this.Kill(this, e);
            }
        }

        /// <summary>
        ///   Occurs when a <see cref="LinksMessage" /> is received.
        /// </summary>
        public event IrcMessageEventHandler<LinksMessage> Links;

        /// <summary>
        ///   Raises the Links event.
        /// </summary>
        protected internal void OnLinks(IrcMessageEventArgs<LinksMessage> e)
        {
            if (this.Links != null)
            {
                this.Links(this, e);
            }
        }

        /// <summary>
        ///   Occurs when a <see cref="LinksReplyMessage" /> is received.
        /// </summary>
        public event IrcMessageEventHandler<LinksReplyMessage> LinksReply;

        /// <summary>
        ///   Raises the LinksReply event.
        /// </summary>
        protected internal void OnLinksReply(IrcMessageEventArgs<LinksReplyMessage> e)
        {
            if (this.LinksReply != null)
            {
                this.LinksReply(this, e);
            }
        }

        /// <summary>
        ///   Occurs when a <see cref="LinksEndReplyMessage" /> is received.
        /// </summary>
        public event IrcMessageEventHandler<LinksEndReplyMessage> LinksEndReply;

        /// <summary>
        ///   Raises the LinksEndReply event.
        /// </summary>
        protected internal void OnLinksEndReply(IrcMessageEventArgs<LinksEndReplyMessage> e)
        {
            if (this.LinksEndReply != null)
            {
                this.LinksEndReply(this, e);
            }
        }

        /// <summary>
        ///   Occurs when a <see cref="ListMessage" /> is received.
        /// </summary>
        public event IrcMessageEventHandler<ListMessage> List;

        /// <summary>
        ///   Raises the List event.
        /// </summary>
        protected internal void OnList(IrcMessageEventArgs<ListMessage> e)
        {
            if (this.List != null)
            {
                this.List(this, e);
            }
        }

        /// <summary>
        ///   Occurs when a <see cref="ListStartReplyMessage" /> is received.
        /// </summary>
        public event IrcMessageEventHandler<ListStartReplyMessage> ListStartReply;

        /// <summary>
        ///   Raises the ListStartReply event.
        /// </summary>
        protected internal void OnListStartReply(IrcMessageEventArgs<ListStartReplyMessage> e)
        {
            if (this.ListStartReply != null)
            {
                this.ListStartReply(this, e);
            }
        }

        /// <summary>
        ///   Occurs when a <see cref="ListReplyMessage" /> is received.
        /// </summary>
        public event IrcMessageEventHandler<ListReplyMessage> ListReply;

        /// <summary>
        ///   Raises the ListReply event.
        /// </summary>
        protected internal void OnListReply(IrcMessageEventArgs<ListReplyMessage> e)
        {
            if (this.ListReply != null)
            {
                this.ListReply(this, e);
            }
        }

        /// <summary>
        ///   Occurs when a <see cref="ListEndReplyMessage" /> is received.
        /// </summary>
        public event IrcMessageEventHandler<ListEndReplyMessage> ListEndReply;

        /// <summary>
        ///   Raises the ListEndReply event.
        /// </summary>
        protected internal void OnListEndReply(IrcMessageEventArgs<ListEndReplyMessage> e)
        {
            if (this.ListEndReply != null)
            {
                this.ListEndReply(this, e);
            }
        }

        /// <summary>
        ///   Occurs when a <see cref="LusersMessage" /> is received.
        /// </summary>
        public event IrcMessageEventHandler<LusersMessage> Lusers;

        /// <summary>
        ///   Raises the Lusers event.
        /// </summary>
        protected internal void OnLusers(IrcMessageEventArgs<LusersMessage> e)
        {
            if (this.Lusers != null)
            {
                this.Lusers(this, e);
            }
        }

        /// <summary>
        ///   Occurs when a <see cref="LusersReplyMessage" /> is received.
        /// </summary>
        public event IrcMessageEventHandler<LusersReplyMessage> LusersReply;

        /// <summary>
        ///   Raises the LusersReply event.
        /// </summary>
        protected internal void OnLusersReply(IrcMessageEventArgs<LusersReplyMessage> e)
        {
            if (this.LusersReply != null)
            {
                this.LusersReply(this, e);
            }
        }

        /// <summary>
        ///   Occurs when a <see cref="LusersOpReplyMessage" /> is received.
        /// </summary>
        public event IrcMessageEventHandler<LusersOpReplyMessage> LusersOpReply;

        /// <summary>
        ///   Raises the LusersOpReply event.
        /// </summary>
        protected internal void OnLusersOpReply(IrcMessageEventArgs<LusersOpReplyMessage> e)
        {
            if (this.LusersOpReply != null)
            {
                this.LusersOpReply(this, e);
            }
        }

        /// <summary>
        ///   Occurs when a <see cref="LusersMeReplyMessage" /> is received.
        /// </summary>
        public event IrcMessageEventHandler<LusersMeReplyMessage> LusersMeReply;

        /// <summary>
        ///   Raises the LusersMeReply event.
        /// </summary>
        protected internal void OnLusersMeReply(IrcMessageEventArgs<LusersMeReplyMessage> e)
        {
            if (this.LusersMeReply != null)
            {
                this.LusersMeReply(this, e);
            }
        }

        /// <summary>
        ///   Occurs when a <see cref="LusersChannelsReplyMessage" /> is received.
        /// </summary>
        public event IrcMessageEventHandler<LusersChannelsReplyMessage> LusersChannelsReply;

        /// <summary>
        ///   Raises the LusersChannelsReply event.
        /// </summary>
        protected internal void OnLusersChannelsReply(IrcMessageEventArgs<LusersChannelsReplyMessage> e)
        {
            if (this.LusersChannelsReply != null)
            {
                this.LusersChannelsReply(this, e);
            }
        }

        /// <summary>
        ///   Occurs when a <see cref="MotdMessage" /> is received.
        /// </summary>
        public event IrcMessageEventHandler<MotdMessage> Motd;

        /// <summary>
        ///   Raises the Motd event.
        /// </summary>
        protected internal void OnMotd(IrcMessageEventArgs<MotdMessage> e)
        {
            if (this.Motd != null)
            {
                this.Motd(this, e);
            }
        }

        /// <summary>
        ///   Occurs when a <see cref="MotdStartReplyMessage" /> is received.
        /// </summary>
        public event IrcMessageEventHandler<MotdStartReplyMessage> MotdStartReply;

        /// <summary>
        ///   Raises the MotdStartReply event.
        /// </summary>
        protected internal void OnMotdStartReply(IrcMessageEventArgs<MotdStartReplyMessage> e)
        {
            if (this.MotdStartReply != null)
            {
                this.MotdStartReply(this, e);
            }
        }

        /// <summary>
        ///   Occurs when a <see cref="MotdReplyMessage" /> is received.
        /// </summary>
        public event IrcMessageEventHandler<MotdReplyMessage> MotdReply;

        /// <summary>
        ///   Raises the MotdReply event.
        /// </summary>
        protected internal void OnMotdReply(IrcMessageEventArgs<MotdReplyMessage> e)
        {
            if (this.MotdReply != null)
            {
                this.MotdReply(this, e);
            }
        }

        /// <summary>
        ///   Occurs when a <see cref="MotdEndReplyMessage" /> is received.
        /// </summary>
        public event IrcMessageEventHandler<MotdEndReplyMessage> MotdEndReply;

        /// <summary>
        ///   Raises the MotdEndReply event.
        /// </summary>
        protected internal void OnMotdEndReply(IrcMessageEventArgs<MotdEndReplyMessage> e)
        {
            if (this.MotdEndReply != null)
            {
                this.MotdEndReply(this, e);
            }
        }

        /// <summary>
        ///   Occurs when a <see cref="NamesMessage" /> is received.
        /// </summary>
        public event IrcMessageEventHandler<NamesMessage> Names;

        /// <summary>
        ///   Raises the Names event.
        /// </summary>
        protected internal void OnNames(IrcMessageEventArgs<NamesMessage> e)
        {
            if (this.Names != null)
            {
                this.Names(this, e);
            }
        }

        /// <summary>
        ///   Occurs when a <see cref="NamesReplyMessage" /> is received.
        /// </summary>
        public event IrcMessageEventHandler<NamesReplyMessage> NamesReply;

        /// <summary>
        ///   Raises the NamesReply event.
        /// </summary>
        protected internal void OnNamesReply(IrcMessageEventArgs<NamesReplyMessage> e)
        {
            if (this.NamesReply != null)
            {
                this.NamesReply(this, e);
            }
        }

        /// <summary>
        ///   Occurs when a <see cref="NamesEndReplyMessage" /> is received.
        /// </summary>
        public event IrcMessageEventHandler<NamesEndReplyMessage> NamesEndReply;

        /// <summary>
        ///   Raises the NamesEndReply event.
        /// </summary>
        protected internal void OnNamesEndReply(IrcMessageEventArgs<NamesEndReplyMessage> e)
        {
            if (this.NamesEndReply != null)
            {
                this.NamesEndReply(this, e);
            }
        }

        /// <summary>
        ///   Occurs when a <see cref="OperMessage" /> is received.
        /// </summary>
        public event IrcMessageEventHandler<OperMessage> Oper;

        /// <summary>
        ///   Raises the Oper event.
        /// </summary>
        protected internal void OnOper(IrcMessageEventArgs<OperMessage> e)
        {
            if (this.Oper != null)
            {
                this.Oper(this, e);
            }
        }

        /// <summary>
        ///   Occurs when a <see cref="OperReplyMessage" /> is received.
        /// </summary>
        public event IrcMessageEventHandler<OperReplyMessage> OperReply;

        /// <summary>
        ///   Raises the OperReply event.
        /// </summary>
        protected internal void OnOperReply(IrcMessageEventArgs<OperReplyMessage> e)
        {
            if (this.OperReply != null)
            {
                this.OperReply(this, e);
            }
        }

        /// <summary>
        ///   Occurs when a <see cref="PasswordMessage" /> is received.
        /// </summary>
        public event IrcMessageEventHandler<PasswordMessage> Password;

        /// <summary>
        ///   Raises the Password event.
        /// </summary>
        protected internal void OnPassword(IrcMessageEventArgs<PasswordMessage> e)
        {
            if (this.Password != null)
            {
                this.Password(this, e);
            }
        }

        /// <summary>
        ///   Occurs when a <see cref="SelfAwayMessage" /> is received.
        /// </summary>
        public event IrcMessageEventHandler<SelfAwayMessage> SelfAway;

        /// <summary>
        ///   Raises the SelfAway event.
        /// </summary>
        protected internal void OnSelfAway(IrcMessageEventArgs<SelfAwayMessage> e)
        {
            if (this.SelfAway != null)
            {
                this.SelfAway(this, e);
            }
        }

        /// <summary>
        ///   Occurs when a <see cref="SelfUnAwayMessage" /> is received.
        /// </summary>
        public event IrcMessageEventHandler<SelfUnAwayMessage> SelfUnAway;

        /// <summary>
        ///   Raises the SelfUnAway event.
        /// </summary>
        protected internal void OnSelfUnAway(IrcMessageEventArgs<SelfUnAwayMessage> e)
        {
            if (this.SelfUnAway != null)
            {
                this.SelfUnAway(this, e);
            }
        }

        /// <summary>
        ///   Occurs when a <see cref="StatsMessage" /> is received.
        /// </summary>
        public event IrcMessageEventHandler<StatsMessage> Stats;

        /// <summary>
        ///   Raises the Stats event.
        /// </summary>
        protected internal void OnStats(IrcMessageEventArgs<StatsMessage> e)
        {
            if (this.Stats != null)
            {
                this.Stats(this, e);
            }
        }

        /// <summary>
        ///   Occurs when a <see cref="StatsReplyMessage" /> is received.
        /// </summary>
        public event IrcMessageEventHandler<StatsReplyMessage> StatsReply;

        /// <summary>
        ///   Raises the StatsReply event.
        /// </summary>
        protected internal void OnStatsReply(IrcMessageEventArgs<StatsReplyMessage> e)
        {
            if (this.StatsReply != null)
            {
                this.StatsReply(this, e);
            }
        }

        /// <summary>
        ///   Occurs when a <see cref="TimeMessage" /> is received.
        /// </summary>
        public event IrcMessageEventHandler<TimeMessage> Time;

        /// <summary>
        ///   Raises the Time event.
        /// </summary>
        protected internal void OnTime(IrcMessageEventArgs<TimeMessage> e)
        {
            if (this.Time != null)
            {
                this.Time(this, e);
            }
        }

        /// <summary>
        ///   Occurs when a <see cref="ServerTimeReplyMessage" /> is received.
        /// </summary>
        public event IrcMessageEventHandler<ServerTimeReplyMessage> ServerTimeReply;

        /// <summary>
        ///   Raises the ServerTimeReply event.
        /// </summary>
        protected internal void OnServerTimeReply(IrcMessageEventArgs<ServerTimeReplyMessage> e)
        {
            if (this.ServerTimeReply != null)
            {
                this.ServerTimeReply(this, e);
            }
        }

        /// <summary>
        ///   Occurs when a <see cref="TraceMessage" /> is received.
        /// </summary>
        public event IrcMessageEventHandler<TraceMessage> Trace;

        /// <summary>
        ///   Raises the Trace event.
        /// </summary>
        protected internal void OnTrace(IrcMessageEventArgs<TraceMessage> e)
        {
            if (this.Trace != null)
            {
                this.Trace(this, e);
            }
        }

        /// <summary>
        ///   Occurs when a <see cref="UserNotificationMessage" /> is received.
        /// </summary>
        public event IrcMessageEventHandler<UserNotificationMessage> UserNotification;

        /// <summary>
        ///   Raises the UserNotification event.
        /// </summary>
        protected internal void OnUserNotification(IrcMessageEventArgs<UserNotificationMessage> e)
        {
            if (this.UserNotification != null)
            {
                this.UserNotification(this, e);
            }
        }

        /// <summary>
        ///   Occurs when a <see cref="UserNotificationServerSideMessage" /> is received.
        /// </summary>
        public event IrcMessageEventHandler<UserNotificationServerSideMessage> UserNotificationServerSide;

        /// <summary>
        ///   Raises the <see cref="UserNotificationServerSide" /> event.
        /// </summary>
        protected internal void OnUserNotificationServerSide(IrcMessageEventArgs<UserNotificationServerSideMessage> ircMessageEventArgs)
        {
            if (this.UserNotificationServerSide != null)
            {
                this.UserNotificationServerSide(this, ircMessageEventArgs);
            }
        }

        /// <summary>
        ///   Occurs when a <see cref="UserAwayMessage" /> is received.
        /// </summary>
        public event IrcMessageEventHandler<UserAwayMessage> UserAway;

        /// <summary>
        ///   Raises the UserAway event.
        /// </summary>
        protected internal void OnUserAway(IrcMessageEventArgs<UserAwayMessage> e)
        {
            if (this.UserAway != null)
            {
                this.UserAway(this, e);
            }
        }

        /// <summary>
        ///   Occurs when a <see cref="VersionMessage" /> is received.
        /// </summary>
        public event IrcMessageEventHandler<VersionMessage> Version;

        /// <summary>
        ///   Raises the Version event.
        /// </summary>
        protected internal void OnVersion(IrcMessageEventArgs<VersionMessage> e)
        {
            if (this.Version != null)
            {
                this.Version(this, e);
            }
        }

        /// <summary>
        ///   Occurs when a <see cref="WallopsMessage" /> is received.
        /// </summary>
        public event IrcMessageEventHandler<WallopsMessage> Wallops;

        /// <summary>
        ///   Raises the Wallops event.
        /// </summary>
        protected internal void OnWallops(IrcMessageEventArgs<WallopsMessage> e)
        {
            if (this.Wallops != null)
            {
                this.Wallops(this, e);
            }
        }

        /// <summary>
        ///   Occurs when a <see cref="WallchopsMessage" /> is received.
        /// </summary>
        public event IrcMessageEventHandler<WallchopsMessage> Wallchops;

        /// <summary>
        ///   Raises the Wallchops event.
        /// </summary>
        protected internal void OnWallchops(IrcMessageEventArgs<WallchopsMessage> e)
        {
            if (this.Wallchops != null)
            {
                this.Wallchops(this, e);
            }
        }

        /// <summary>
        ///   Occurs when a <see cref="WhoMessage" /> is received.
        /// </summary>
        public event IrcMessageEventHandler<WhoMessage> Who;

        /// <summary>
        ///   Raises the Who event.
        /// </summary>
        protected internal void OnWho(IrcMessageEventArgs<WhoMessage> e)
        {
            if (this.Who != null)
            {
                this.Who(this, e);
            }
        }

        /// <summary>
        ///   Occurs when a <see cref="WhoReplyMessage" /> is received.
        /// </summary>
        public event IrcMessageEventHandler<WhoReplyMessage> WhoReply;

        /// <summary>
        ///   Raises the WhoReply event.
        /// </summary>
        protected internal void OnWhoReply(IrcMessageEventArgs<WhoReplyMessage> e)
        {
            if (this.WhoReply != null)
            {
                this.WhoReply(this, e);
            }
        }

        /// <summary>
        ///   Occurs when a <see cref="WhoEndReplyMessage" /> is received.
        /// </summary>
        public event IrcMessageEventHandler<WhoEndReplyMessage> WhoEndReply;

        /// <summary>
        ///   Raises the WhoEndReply event.
        /// </summary>
        protected internal void OnWhoEndReply(IrcMessageEventArgs<WhoEndReplyMessage> e)
        {
            if (this.WhoEndReply != null)
            {
                this.WhoEndReply(this, e);
            }
        }

        /// <summary>
        ///   Occurs when a <see cref="WhoIsMessage" /> is received.
        /// </summary>
        public event IrcMessageEventHandler<WhoIsMessage> WhoIs;

        /// <summary>
        ///   Raises the WhoIs event.
        /// </summary>
        protected internal void OnWhoIs(IrcMessageEventArgs<WhoIsMessage> e)
        {
            if (this.WhoIs != null)
            {
                this.WhoIs(this, e);
            }
        }

        /// <summary>
        ///   Occurs when a <see cref="WhoIsChannelsReplyMessage" /> is received.
        /// </summary>
        public event IrcMessageEventHandler<WhoIsChannelsReplyMessage> WhoIsChannelsReply;

        /// <summary>
        ///   Raises the WhoIsChannelsReply event.
        /// </summary>
        protected internal void OnWhoIsChannelsReply(IrcMessageEventArgs<WhoIsChannelsReplyMessage> e)
        {
            if (this.WhoIsChannelsReply != null)
            {
                this.WhoIsChannelsReply(this, e);
            }
        }

        /// <summary>
        ///   Occurs when a <see cref="WhoIsIdleReplyMessage" /> is received.
        /// </summary>
        public event IrcMessageEventHandler<WhoIsIdleReplyMessage> WhoIsIdleReply;

        /// <summary>
        ///   Raises the WhoIsIdleReply event.
        /// </summary>
        protected internal void OnWhoIsIdleReply(IrcMessageEventArgs<WhoIsIdleReplyMessage> e)
        {
            if (this.WhoIsIdleReply != null)
            {
                this.WhoIsIdleReply(this, e);
            }
        }

        /// <summary>
        ///   Occurs when a <see cref="WhoIsOperReplyMessage" /> is received.
        /// </summary>
        public event IrcMessageEventHandler<WhoIsOperReplyMessage> WhoIsOperReply;

        /// <summary>
        ///   Raises the WhoIsOperReply event.
        /// </summary>
        protected internal void OnWhoIsOperReply(IrcMessageEventArgs<WhoIsOperReplyMessage> e)
        {
            if (this.WhoIsOperReply != null)
            {
                this.WhoIsOperReply(this, e);
            }
        }

        /// <summary>
        ///   Occurs when a <see cref="WhoIsServerReplyMessage" /> is received.
        /// </summary>
        public event IrcMessageEventHandler<WhoIsServerReplyMessage> WhoIsServerReply;

        /// <summary>
        ///   Raises the WhoIsServerReply event.
        /// </summary>
        protected internal void OnWhoIsServerReply(IrcMessageEventArgs<WhoIsServerReplyMessage> e)
        {
            if (this.WhoIsServerReply != null)
            {
                this.WhoIsServerReply(this, e);
            }
        }

        /// <summary>
        ///   Occurs when a <see cref="WhoIsUserReplyMessage" /> is received.
        /// </summary>
        public event IrcMessageEventHandler<WhoIsUserReplyMessage> WhoIsUserReply;

        /// <summary>
        ///   Raises the WhoIsUserReply event.
        /// </summary>
        protected internal void OnWhoIsUserReply(IrcMessageEventArgs<WhoIsUserReplyMessage> e)
        {
            if (this.WhoIsUserReply != null)
            {
                this.WhoIsUserReply(this, e);
            }
        }

        /// <summary>
        ///   Occurs when a <see cref="WhoIsEndReplyMessage" /> is received.
        /// </summary>
        public event IrcMessageEventHandler<WhoIsEndReplyMessage> WhoIsEndReply;

        /// <summary>
        ///   Raises the WhoIsEndReply event.
        /// </summary>
        protected internal void OnWhoIsEndReply(IrcMessageEventArgs<WhoIsEndReplyMessage> e)
        {
            if (this.WhoIsEndReply != null)
            {
                this.WhoIsEndReply(this, e);
            }
        }

        /// <summary>
        ///   Occurs when a <see cref="WhoIsRegisteredNickReplyMessage" /> is received.
        /// </summary>
        public event IrcMessageEventHandler<WhoIsRegisteredNickReplyMessage> WhoIsRegisteredNickReply;

        /// <summary>
        ///   Raises the <see cref="WhoIsRegisteredNickReply" /> event.
        /// </summary>
        protected internal void OnWhoIsRegisteredNickReply(IrcMessageEventArgs<WhoIsRegisteredNickReplyMessage> ircMessageEventArgs)
        {
            if (this.WhoIsRegisteredNickReply != null)
            {
                this.WhoIsRegisteredNickReply(this, ircMessageEventArgs);
            }
        }

        /// <summary>
        ///   Occurs when a <see cref="WhoWasMessage" /> is received.
        /// </summary>
        public event IrcMessageEventHandler<WhoWasMessage> WhoWas;

        /// <summary>
        ///   Raises the WhoWas event.
        /// </summary>
        protected internal void OnWhoWas(IrcMessageEventArgs<WhoWasMessage> e)
        {
            if (this.WhoWas != null)
            {
                this.WhoWas(this, e);
            }
        }

        /// <summary>
        ///   Occurs when a <see cref="WhoWasUserReplyMessage" /> is received.
        /// </summary>
        public event IrcMessageEventHandler<WhoWasUserReplyMessage> WhoWasUserReply;

        /// <summary>
        ///   Raises the WhoWasUserReply event.
        /// </summary>
        protected internal void OnWhoWasUserReply(IrcMessageEventArgs<WhoWasUserReplyMessage> e)
        {
            if (this.WhoWasUserReply != null)
            {
                this.WhoWasUserReply(this, e);
            }
        }

        /// <summary>
        ///   Occurs when a <see cref="WhoWasEndReplyMessage" /> is received.
        /// </summary>
        public event IrcMessageEventHandler<WhoWasEndReplyMessage> WhoWasEndReply;

        /// <summary>
        ///   Raises the WhoWasEndReply event.
        /// </summary>
        protected internal void OnWhoWasEndReply(IrcMessageEventArgs<WhoWasEndReplyMessage> e)
        {
            if (this.WhoWasEndReply != null)
            {
                this.WhoWasEndReply(this, e);
            }
        }

        /// <summary>
        ///   Occurs when a <see cref="UserHostMessage" /> is received.
        /// </summary>
        public event IrcMessageEventHandler<UserHostMessage> UserHost;

        /// <summary>
        ///   Raises the UserHost event.
        /// </summary>
        protected internal void OnUserHost(IrcMessageEventArgs<UserHostMessage> e)
        {
            if (this.UserHost != null)
            {
                this.UserHost(this, e);
            }
        }

        /// <summary>
        ///   Occurs when a <see cref="UserHostReplyMessage" /> is received.
        /// </summary>
        public event IrcMessageEventHandler<UserHostReplyMessage> UserHostReply;

        /// <summary>
        ///   Raises the UserHostReply event.
        /// </summary>
        protected internal void OnUserHostReply(IrcMessageEventArgs<UserHostReplyMessage> e)
        {
            if (this.UserHostReply != null)
            {
                this.UserHostReply(this, e);
            }
        }

        /// <summary>
        ///   Occurs when a <see cref="SilenceMessage" /> is received.
        /// </summary>
        public event IrcMessageEventHandler<SilenceMessage> Silence;

        /// <summary>
        ///   Raises the Silence event.
        /// </summary>
        protected internal void OnSilence(IrcMessageEventArgs<SilenceMessage> e)
        {
            if (this.Silence != null)
            {
                this.Silence(this, e);
            }
        }

        /// <summary>
        ///   Occurs when a <see cref="SilenceReplyMessage" /> is received.
        /// </summary>
        public event IrcMessageEventHandler<SilenceReplyMessage> SilenceReply;

        /// <summary>
        ///   Raises the SilenceReply event.
        /// </summary>
        protected internal void OnSilenceReply(IrcMessageEventArgs<SilenceReplyMessage> e)
        {
            if (this.SilenceReply != null)
            {
                this.SilenceReply(this, e);
            }
        }

        /// <summary>
        ///   Occurs when a <see cref="SilenceEndReplyMessage" /> is received.
        /// </summary>
        public event IrcMessageEventHandler<SilenceEndReplyMessage> SilenceEndReply;

        /// <summary>
        ///   Raises the SilenceEndReply event.
        /// </summary>
        protected internal void OnSilenceEndReply(IrcMessageEventArgs<SilenceEndReplyMessage> e)
        {
            if (this.SilenceEndReply != null)
            {
                this.SilenceEndReply(this, e);
            }
        }

        /// <summary>
        ///   Occurs when a <see cref="AcceptListEditorMessage" /> is received.
        /// </summary>
        public event IrcMessageEventHandler<AcceptListEditorMessage> AcceptListEditor;

        /// <summary>
        ///   Raises the <see cref="AcceptListEditor" /> event.
        /// </summary>
        protected internal void OnAcceptListEditor(IrcMessageEventArgs<AcceptListEditorMessage> ircMessageEventArgs)
        {
            if (this.AcceptListEditor != null)
            {
                this.AcceptListEditor(this, ircMessageEventArgs);
            }
        }

        /// <summary>
        ///   Occurs when a <see cref="AcceptListRequestMessage" /> is received.
        /// </summary>
        public event IrcMessageEventHandler<AcceptListRequestMessage> AcceptListRequest;

        /// <summary>
        ///   Raises the <see cref="AcceptListRequest" /> event.
        /// </summary>
        protected internal void OnAcceptListRequest(IrcMessageEventArgs<AcceptListRequestMessage> ircMessageEventArgs)
        {
            if (this.AcceptListRequest != null)
            {
                this.AcceptListRequest(this, ircMessageEventArgs);
            }
        }

        /// <summary>
        ///   Occurs when a <see cref="AcceptListReplyMessage" /> is received.
        /// </summary>
        public event IrcMessageEventHandler<AcceptListReplyMessage> AcceptListReply;

        /// <summary>
        ///   Raises the <see cref="AcceptListReply" /> event.
        /// </summary>
        protected internal void OnAcceptListReply(IrcMessageEventArgs<AcceptListReplyMessage> ircMessageEventArgs)
        {
            if (this.AcceptListReply != null)
            {
                this.AcceptListReply(this, ircMessageEventArgs);
            }
        }

        /// <summary>
        ///   Occurs when a <see cref="AcceptListEndReplyMessage" /> is received.
        /// </summary>
        public event IrcMessageEventHandler<AcceptListEndReplyMessage> AcceptListEndReply;

        /// <summary>
        ///   Raises the <see cref="AcceptListEndReply" /> event.
        /// </summary>
        protected internal void OnAcceptListEndReply(IrcMessageEventArgs<AcceptListEndReplyMessage> ircMessageEventArgs)
        {
            if (this.AcceptListEndReply != null)
            {
                this.AcceptListEndReply(this, ircMessageEventArgs);
            }
        }

        #endregion


        #region CTCP

        /// <summary>
        ///   Occurs when a <see cref="ActionRequestMessage" /> is received.
        /// </summary>
        public event IrcMessageEventHandler<ActionRequestMessage> ActionRequest;

        /// <summary>
        ///   Raises the ActionRequest event.
        /// </summary>
        protected internal void OnActionRequest(IrcMessageEventArgs<ActionRequestMessage> e)
        {
            if (this.ActionRequest != null)
            {
                this.ActionRequest(this, e);
            }
        }

        /// <summary>
        ///   Occurs when a <see cref="GenericCtcpReplyMessage" /> is received.
        /// </summary>
        public event IrcMessageEventHandler<GenericCtcpRequestMessage> GenericCtcpRequest;

        /// <summary>
        ///   Raises the GenericCtcpReply event.
        /// </summary>
        protected internal void OnGenericCtcpRequest(IrcMessageEventArgs<GenericCtcpRequestMessage> e)
        {
            if (this.GenericCtcpRequest != null)
            {
                this.GenericCtcpRequest(this, e);
            }
        }

        /// <summary>
        ///   Occurs when a <see cref="GenericCtcpRequestMessage" /> is received.
        /// </summary>
        public event IrcMessageEventHandler<GenericCtcpReplyMessage> GenericCtcpReply;

        /// <summary>
        ///   Raises the GenericCtcpRequest event.
        /// </summary>
        protected internal void OnGenericCtcpReply(IrcMessageEventArgs<GenericCtcpReplyMessage> e)
        {
            if (this.GenericCtcpReply != null)
            {
                this.GenericCtcpReply(this, e);
            }
        }

        /// <summary>
        ///   Occurs when a <see cref="ClientInfoRequestMessage" /> is received.
        /// </summary>
        public event IrcMessageEventHandler<ClientInfoRequestMessage> ClientInfoRequest;

        /// <summary>
        ///   Raises the ClientInfoRequest event.
        /// </summary>
        protected internal void OnClientInfoRequest(IrcMessageEventArgs<ClientInfoRequestMessage> e)
        {
            if (this.ClientInfoRequest != null)
            {
                this.ClientInfoRequest(this, e);
            }
        }

        /// <summary>
        ///   Occurs when a <see cref="ClientInfoReplyMessage" /> is received.
        /// </summary>
        public event IrcMessageEventHandler<ClientInfoReplyMessage> ClientInfoReply;

        /// <summary>
        ///   Raises the ClientInfoReply event.
        /// </summary>
        protected internal void OnClientInfoReply(IrcMessageEventArgs<ClientInfoReplyMessage> e)
        {
            if (this.ClientInfoReply != null)
            {
                this.ClientInfoReply(this, e);
            }
        }

        /// <summary>
        ///   Occurs when a <see cref="FingerRequestMessage" /> is received.
        /// </summary>
        public event IrcMessageEventHandler<FingerRequestMessage> FingerRequest;

        /// <summary>
        ///   Raises the FingerRequest event.
        /// </summary>
        protected internal void OnFingerRequest(IrcMessageEventArgs<FingerRequestMessage> e)
        {
            if (this.FingerRequest != null)
            {
                this.FingerRequest(this, e);
            }
        }

        /// <summary>
        ///   Occurs when a <see cref="FingerReplyMessage" /> is received.
        /// </summary>
        public event IrcMessageEventHandler<FingerReplyMessage> FingerReply;

        /// <summary>
        ///   Raises the FingerReply event.
        /// </summary>
        protected internal void OnFingerReply(IrcMessageEventArgs<FingerReplyMessage> e)
        {
            if (this.FingerReply != null)
            {
                this.FingerReply(this, e);
            }
        }

        /// <summary>
        ///   Occurs when a <see cref="PageRequestMessage" /> is received.
        /// </summary>
        public event IrcMessageEventHandler<PageRequestMessage> PageRequest;

        /// <summary>
        ///   Raises the PageRequest event.
        /// </summary>
        protected internal void OnPageRequest(IrcMessageEventArgs<PageRequestMessage> e)
        {
            if (this.PageRequest != null)
            {
                this.PageRequest(this, e);
            }
        }

        /// <summary>
        ///   Occurs when a <see cref="ScriptRequestMessage" /> is received.
        /// </summary>
        public event IrcMessageEventHandler<ScriptRequestMessage> ScriptRequest;

        /// <summary>
        ///   Raises the ScriptRequest event.
        /// </summary>
        protected internal void OnScriptRequest(IrcMessageEventArgs<ScriptRequestMessage> e)
        {
            if (this.ScriptRequest != null)
            {
                this.ScriptRequest(this, e);
            }
        }

        /// <summary>
        ///   Occurs when a <see cref="ScriptReplyMessage" /> is received.
        /// </summary>
        public event IrcMessageEventHandler<ScriptReplyMessage> ScriptReply;

        /// <summary>
        ///   Raises the ScriptReply event.
        /// </summary>
        protected internal void OnScriptReply(IrcMessageEventArgs<ScriptReplyMessage> e)
        {
            if (this.ScriptReply != null)
            {
                this.ScriptReply(this, e);
            }
        }

        /// <summary>
        ///   Occurs when a <see cref="PingRequestMessage" /> is received.
        /// </summary>
        public event IrcMessageEventHandler<PingRequestMessage> PingRequest;

        /// <summary>
        ///   Raises the PingRequest event.
        /// </summary>
        protected internal void OnPingRequest(IrcMessageEventArgs<PingRequestMessage> e)
        {
            if (this.PingRequest != null)
            {
                this.PingRequest(this, e);
            }
        }

        /// <summary>
        ///   Occurs when a <see cref="PingReplyMessage" /> is received.
        /// </summary>
        public event IrcMessageEventHandler<PingReplyMessage> PingReply;

        /// <summary>
        ///   Raises the PingReply event.
        /// </summary>
        protected internal void OnPingReply(IrcMessageEventArgs<PingReplyMessage> e)
        {
            if (this.PingReply != null)
            {
                this.PingReply(this, e);
            }
        }

        /// <summary>
        ///   Occurs when a <see cref="TimeRequestMessage" /> is received.
        /// </summary>
        public event IrcMessageEventHandler<TimeRequestMessage> TimeRequest;

        /// <summary>
        ///   Raises the TimeRequest event.
        /// </summary>
        protected internal void OnTimeRequest(IrcMessageEventArgs<TimeRequestMessage> e)
        {
            if (this.TimeRequest != null)
            {
                this.TimeRequest(this, e);
            }
        }

        /// <summary>
        ///   Occurs when a <see cref="TimeReplyMessage" /> is received.
        /// </summary>
        public event IrcMessageEventHandler<TimeReplyMessage> TimeReply;

        /// <summary>
        ///   Raises the TimeReply event.
        /// </summary>
        protected internal void OnTimeReply(IrcMessageEventArgs<TimeReplyMessage> e)
        {
            if (this.TimeReply != null)
            {
                this.TimeReply(this, e);
            }
        }

        /// <summary>
        ///   Occurs when a <see cref="VersionRequestMessage" /> is received.
        /// </summary>
        public event IrcMessageEventHandler<VersionRequestMessage> VersionRequest;

        /// <summary>
        ///   Raises the VersionRequest event.
        /// </summary>
        protected internal void OnVersionRequest(IrcMessageEventArgs<VersionRequestMessage> e)
        {
            if (this.VersionRequest != null)
            {
                this.VersionRequest(this, e);
            }
        }

        /// <summary>
        ///   Occurs when a <see cref="VersionReplyMessage" /> is received.
        /// </summary>
        public event IrcMessageEventHandler<VersionReplyMessage> VersionReply;

        /// <summary>
        ///   Raises the VersionReply event.
        /// </summary>
        protected internal void OnVersionReply(IrcMessageEventArgs<VersionReplyMessage> e)
        {
            if (this.VersionReply != null)
            {
                this.VersionReply(this, e);
            }
        }

        /// <summary>
        ///   Occurs when a <see cref="UserInfoRequestMessage" /> is received.
        /// </summary>
        public event IrcMessageEventHandler<UserInfoRequestMessage> UserInfoRequest;

        /// <summary>
        ///   Raises the UserInfoRequest event.
        /// </summary>
        protected internal void OnUserInfoRequest(IrcMessageEventArgs<UserInfoRequestMessage> e)
        {
            if (this.UserInfoRequest != null)
            {
                this.UserInfoRequest(this, e);
            }
        }

        /// <summary>
        ///   Occurs when a <see cref="UserInfoReplyMessage" /> is received.
        /// </summary>
        public event IrcMessageEventHandler<UserInfoReplyMessage> UserInfoReply;

        /// <summary>
        ///   Raises the UserInfoReply event.
        /// </summary>
        protected internal void OnUserInfoReply(IrcMessageEventArgs<UserInfoReplyMessage> e)
        {
            if (this.UserInfoReply != null)
            {
                this.UserInfoReply(this, e);
            }
        }

        /// <summary>
        ///   Occurs when a <see cref="SourceRequestMessage" /> is received.
        /// </summary>
        public event IrcMessageEventHandler<SourceRequestMessage> SourceRequest;

        /// <summary>
        ///   Raises the SourceRequest event.
        /// </summary>
        protected internal void OnSourceRequest(IrcMessageEventArgs<SourceRequestMessage> e)
        {
            if (this.SourceRequest != null)
            {
                this.SourceRequest(this, e);
            }
        }

        /// <summary>
        ///   Occurs when a <see cref="SourceReplyMessage" /> is received.
        /// </summary>
        public event IrcMessageEventHandler<SourceReplyMessage> SourceReply;

        /// <summary>
        ///   Raises the SourceReply event.
        /// </summary>
        protected internal void OnSourceReply(IrcMessageEventArgs<SourceReplyMessage> e)
        {
            if (this.SourceReply != null)
            {
                this.SourceReply(this, e);
            }
        }

        /// <summary>
        ///   Occurs when a <see cref="SoundRequestMessage" /> is received.
        /// </summary>
        public event IrcMessageEventHandler<SoundRequestMessage> SoundRequest;

        /// <summary>
        ///   Raises the SoundRequest event.
        /// </summary>
        protected internal void OnSoundRequest(IrcMessageEventArgs<SoundRequestMessage> e)
        {
            if (this.SoundRequest != null)
            {
                this.SoundRequest(this, e);
            }
        }

        /// <summary>
        ///   Occurs when a <see cref="ErrorReplyMessage" /> is received.
        /// </summary>
        public event IrcMessageEventHandler<ErrorReplyMessage> ErrorReply;

        /// <summary>
        ///   Raises the ErrorReply event.
        /// </summary>
        protected internal void OnErrorReply(IrcMessageEventArgs<ErrorReplyMessage> e)
        {
            if (this.ErrorReply != null)
            {
                this.ErrorReply(this, e);
            }
        }

        /// <summary>
        ///   Occurs when a <see cref="ErrorRequestMessage" /> is received.
        /// </summary>
        public event IrcMessageEventHandler<ErrorRequestMessage> ErrorRequest;

        /// <summary>
        ///   Raises the ErrorRequest event.
        /// </summary>
        protected internal void OnErrorRequest(IrcMessageEventArgs<ErrorRequestMessage> e)
        {
            if (this.ErrorRequest != null)
            {
                this.ErrorRequest(this, e);
            }
        }

        /// <summary>
        ///   Occurs when a <see cref="Mp3RequestMessage" /> is received.
        /// </summary>
        public event IrcMessageEventHandler<Mp3RequestMessage> Mp3Request;

        /// <summary>
        ///   Raises the <see cref="Mp3Request" /> event.
        /// </summary>
        protected internal void OnMp3Request(IrcMessageEventArgs<Mp3RequestMessage> ircMessageEventArgs)
        {
            if (this.Mp3Request != null)
            {
                this.Mp3Request(this, ircMessageEventArgs);
            }
        }

        /// <summary>
        ///   Occurs when a <see cref="SlotsRequestMessage" /> is received.
        /// </summary>
        public event IrcMessageEventHandler<SlotsRequestMessage> SlotsRequest;

        /// <summary>
        ///   Raises the <see cref="SlotsRequest" /> event.
        /// </summary>
        protected internal void OnSlotsRequest(IrcMessageEventArgs<SlotsRequestMessage> ircMessageEventArgs)
        {
            if (this.SlotsRequest != null)
            {
                this.SlotsRequest(this, ircMessageEventArgs);
            }
        }

        #endregion


        #region Dcc

        /// <summary>
        ///   Occurs when a <see cref="DccChatRequestMessage" /> is received.
        /// </summary>
        public event IrcMessageEventHandler<DccChatRequestMessage> DccChatRequest;

        /// <summary>
        ///   Raises the DccChatRequest event.
        /// </summary>
        protected internal void OnDccChatRequest(IrcMessageEventArgs<DccChatRequestMessage> e)
        {
            if (this.DccChatRequest != null)
            {
                this.DccChatRequest(this, e);
            }
        }

        /// <summary>
        ///   Occurs when a <see cref="DccSendRequestMessage" /> is received.
        /// </summary>
        public event IrcMessageEventHandler<DccSendRequestMessage> DccSendRequest;

        /// <summary>
        ///   Raises the DccSendRequest event.
        /// </summary>
        protected internal void OnDccSendRequest(IrcMessageEventArgs<DccSendRequestMessage> e)
        {
            if (this.DccSendRequest != null)
            {
                this.DccSendRequest(this, e);
            }
        }

        /// <summary>
        ///   Occurs when a <see cref="DccGetRequestMessage" /> is received.
        /// </summary>
        public event IrcMessageEventHandler<DccGetRequestMessage> DccGetRequest;

        /// <summary>
        ///   Raises the DccGetRequest event.
        /// </summary>
        protected internal void OnDccGetRequest(IrcMessageEventArgs<DccGetRequestMessage> e)
        {
            if (this.DccGetRequest != null)
            {
                this.DccGetRequest(this, e);
            }
        }

        /// <summary>
        ///   Occurs when a <see cref="DccResumeRequestMessage" /> is received.
        /// </summary>
        public event IrcMessageEventHandler<DccResumeRequestMessage> DccResumeRequest;

        /// <summary>
        ///   Raises the DccResumeRequest event.
        /// </summary>
        protected internal void OnDccResumeRequest(IrcMessageEventArgs<DccResumeRequestMessage> e)
        {
            if (this.DccResumeRequest != null)
            {
                this.DccResumeRequest(this, e);
            }
        }

        /// <summary>
        ///   Occurs when a <see cref="DccAcceptRequestMessage" /> is received.
        /// </summary>
        public event IrcMessageEventHandler<DccAcceptRequestMessage> DccAcceptRequest;

        /// <summary>
        ///   Raises the DccAcceptRequest event.
        /// </summary>
        protected internal void OnDccAcceptRequest(IrcMessageEventArgs<DccAcceptRequestMessage> e)
        {
            if (this.DccAcceptRequest != null)
            {
                this.DccAcceptRequest(this, e);
            }
        }

        #endregion


        #region Modes

        /// <summary>
        ///   Occurs when a <see cref="UserModeMessage" /> is received.
        /// </summary>
        public event IrcMessageEventHandler<UserModeMessage> UserMode;

        /// <summary>
        ///   Raises the UserMode event.
        /// </summary>
        protected internal void OnUserMode(IrcMessageEventArgs<UserModeMessage> e)
        {
            if (this.UserMode != null)
            {
                this.UserMode(this, e);
            }
        }

        /// <summary>
        ///   Occurs when a <see cref="ChannelModeMessage" /> is received.
        /// </summary>
        public event IrcMessageEventHandler<ChannelModeMessage> ChannelMode;

        /// <summary>
        ///   Raises the ChannelMode event.
        /// </summary>
        protected internal void OnChannelMode(IrcMessageEventArgs<ChannelModeMessage> e)
        {
            if (this.ChannelMode != null)
            {
                this.ChannelMode(this, e);
            }
        }

        /// <summary>
        ///   Occurs when a <see cref="ChannelModeIsReplyMessage" /> is received.
        /// </summary>
        public event IrcMessageEventHandler<ChannelModeIsReplyMessage> ChannelModeIsReply;

        /// <summary>
        ///   Raises the ChannelModeIsReply event.
        /// </summary>
        protected internal void OnChannelModeIsReply(IrcMessageEventArgs<ChannelModeIsReplyMessage> e)
        {
            if (this.ChannelModeIsReply != null)
            {
                this.ChannelModeIsReply(this, e);
            }
        }

        /// <summary>
        ///   Occurs when a <see cref="UserModeIsReplyMessage" /> is received.
        /// </summary>
        public event IrcMessageEventHandler<UserModeIsReplyMessage> UserModeIsReply;

        /// <summary>
        ///   Raises the ChannelModeIsReply event.
        /// </summary>
        protected internal void OnUserModeIsReply(IrcMessageEventArgs<UserModeIsReplyMessage> e)
        {
            if (this.UserModeIsReply != null)
            {
                this.UserModeIsReply(this, e);
            }
        }

        /// <summary>
        ///   Occurs when a <see cref="BansReplyMessage" /> is received.
        /// </summary>
        public event IrcMessageEventHandler<BansReplyMessage> BansReply;

        /// <summary>
        ///   Raises the BansReply event.
        /// </summary>
        protected internal void OnBansReply(IrcMessageEventArgs<BansReplyMessage> e)
        {
            if (this.BansReply != null)
            {
                this.BansReply(this, e);
            }
        }

        /// <summary>
        ///   Occurs when a <see cref="BansEndReplyMessage" /> is received.
        /// </summary>
        public event IrcMessageEventHandler<BansEndReplyMessage> BansEndReply;

        /// <summary>
        ///   Raises the BansEndReply event.
        /// </summary>
        protected internal void OnBansEndReply(IrcMessageEventArgs<BansEndReplyMessage> e)
        {
            if (this.BansEndReply != null)
            {
                this.BansEndReply(this, e);
            }
        }

        #endregion


        #region Welcome Messages

        /// <summary>
        ///   Occurs when a <see cref="WelcomeMessage" /> is received.
        /// </summary>
        public event IrcMessageEventHandler<WelcomeMessage> Welcome;

        /// <summary>
        ///   Raises the Welcome event.
        /// </summary>
        protected internal void OnWelcome(IrcMessageEventArgs<WelcomeMessage> e)
        {
            if (this.Welcome != null)
            {
                this.Welcome(this, e);
            }
        }

        /// <summary>
        ///   Occurs when a <see cref="YourHostMessage" /> is received.
        /// </summary>
        public event IrcMessageEventHandler<YourHostMessage> YourHost;

        /// <summary>
        ///   Raises the YourHost event.
        /// </summary>
        protected internal void OnYourHost(IrcMessageEventArgs<YourHostMessage> e)
        {
            if (this.YourHost != null)
            {
                this.YourHost(this, e);
            }
        }

        /// <summary>
        ///   Occurs when a <see cref="ServerCreatedMessage" /> is received.
        /// </summary>
        public event IrcMessageEventHandler<ServerCreatedMessage> ServerCreated;

        /// <summary>
        ///   Raises the ServerCreated event.
        /// </summary>
        protected internal void OnServerCreated(IrcMessageEventArgs<ServerCreatedMessage> e)
        {
            if (this.ServerCreated != null)
            {
                this.ServerCreated(this, e);
            }
        }

        /// <summary>
        ///   Occurs when a <see cref="ServerInfoMessage" /> is received.
        /// </summary>
        public event IrcMessageEventHandler<ServerInfoMessage> ServerInfo;

        /// <summary>
        ///   Raises the ServerInfo event.
        /// </summary>
        protected internal void OnServerInfo(IrcMessageEventArgs<ServerInfoMessage> e)
        {
            if (this.ServerInfo != null)
            {
                this.ServerInfo(this, e);
            }
        }

        /// <summary>
        ///   Occurs when a <see cref="SupportMessage" /> is received.
        /// </summary>
        public event IrcMessageEventHandler<SupportMessage> Support;

        /// <summary>
        ///   Raises the Support event.
        /// </summary>
        protected internal void OnSupport(IrcMessageEventArgs<SupportMessage> e)
        {
            if (this.Support != null)
            {
                this.Support(this, e);
            }
        }

        /// <summary>
        ///   Occurs when a <see cref="UnknownConnectionsMessage" /> is received.
        /// </summary>
        public event IrcMessageEventHandler<UnknownConnectionsMessage> UnknownConnections;

        /// <summary>
        ///   Raises the <see cref="UnknownConnections" /> event.
        /// </summary>
        protected internal void OnUnknownConnections(IrcMessageEventArgs<UnknownConnectionsMessage> ircMessageEventArgs)
        {
            if (this.UnknownConnections != null)
            {
                this.UnknownConnections(this, ircMessageEventArgs);
            }
        }

        /// <summary>
        ///   Occurs when a <see cref="UniqueIdMessage" /> is received.
        /// </summary>
        public event IrcMessageEventHandler<UniqueIdMessage> UniqueId;

        /// <summary>
        ///   Raises the <see cref="UnknownConnections" /> event.
        /// </summary>
        protected internal void OnUniqueId(IrcMessageEventArgs<UniqueIdMessage> ircMessageEventArgs)
        {
            if (this.UniqueId != null)
            {
                this.UniqueId(this, ircMessageEventArgs);
            }
        }

        #endregion


        #region Replies

        /// <summary>
        ///   Occurs when a <see cref="LocalUsersReplyMessage" /> is received.
        /// </summary>
        public event IrcMessageEventHandler<LocalUsersReplyMessage> LocalUsersReply;

        /// <summary>
        ///   Raises the LocalUsersReply event.
        /// </summary>
        protected internal void OnLocalUsersReply(IrcMessageEventArgs<LocalUsersReplyMessage> e)
        {
            if (this.LocalUsersReply != null)
            {
                this.LocalUsersReply(this, e);
            }
        }

        /// <summary>
        ///   Occurs when a <see cref="GlobalUsersReplyMessage" /> is received.
        /// </summary>
        public event IrcMessageEventHandler<GlobalUsersReplyMessage> GlobalUsersReply;

        /// <summary>
        ///   Raises the GlobalUsersReply event.
        /// </summary>
        protected internal void OnGlobalUsersReply(IrcMessageEventArgs<GlobalUsersReplyMessage> e)
        {
            if (this.GlobalUsersReply != null)
            {
                this.GlobalUsersReply(this, e);
            }
        }

        /// <summary>
        ///   Occurs when a <see cref="TopicSetReplyMessage" /> is received.
        /// </summary>
        public event IrcMessageEventHandler<TopicSetReplyMessage> TopicSetReply;

        /// <summary>
        ///   Raises the TopicSetReply event.
        /// </summary>
        protected internal void OnTopicSetReply(IrcMessageEventArgs<TopicSetReplyMessage> e)
        {
            if (this.TopicSetReply != null)
            {
                this.TopicSetReply(this, e);
            }
        }

        /// <summary>
        ///   Occurs when a <see cref="ChannelCreationTimeMessage" /> is received.
        /// </summary>
        public event IrcMessageEventHandler<ChannelCreationTimeMessage> ChannelCreationTime;

        /// <summary>
        ///   Raises the <see cref="ChannelCreationTime" /> event.
        /// </summary>
        protected internal void OnChannelCreationTime(IrcMessageEventArgs<ChannelCreationTimeMessage> ircMessageEventArgs)
        {
            if (this.ChannelCreationTime != null)
            {
                this.ChannelCreationTime(this, ircMessageEventArgs);
            }
        }

        #endregion


        #region IrcX

        /// <summary>
        ///   Occurs when a <see cref="ChannelPropertyMessage" /> is received.
        /// </summary>
        public event IrcMessageEventHandler<ChannelPropertyMessage> ChannelProperty;

        /// <summary>
        ///   Raises the ChannelProperty event.
        /// </summary>
        protected internal void OnChannelProperty(IrcMessageEventArgs<ChannelPropertyMessage> e)
        {
            if (this.ChannelProperty != null)
            {
                this.ChannelProperty(this, e);
            }
        }

        /// <summary>
        ///   Occurs when a <see cref="ChannelPropertyReplyMessage" /> is received.
        /// </summary>
        public event IrcMessageEventHandler<ChannelPropertyReplyMessage> ChannelPropertyReply;

        /// <summary>
        ///   Raises the ChannelPropertyReply event.
        /// </summary>
        protected internal void OnChannelPropertyReply(IrcMessageEventArgs<ChannelPropertyReplyMessage> e)
        {
            if (this.ChannelPropertyReply != null)
            {
                this.ChannelPropertyReply(this, e);
            }
        }

        /// <summary>
        ///   Occurs when a <see cref="ChannelPropertyEndReplyMessage" /> is received.
        /// </summary>
        public event IrcMessageEventHandler<ChannelPropertyEndReplyMessage> ChannelPropertyEndReply;

        /// <summary>
        ///   Raises the ChannelPropertyEndReply event.
        /// </summary>
        protected internal void OnChannelPropertyEndReply(IrcMessageEventArgs<ChannelPropertyEndReplyMessage> e)
        {
            if (this.ChannelPropertyEndReply != null)
            {
                this.ChannelPropertyEndReply(this, e);
            }
        }

        /// <summary>
        ///   Occurs when a <see cref="IrcxMessage" /> is received.
        /// </summary>
        public event IrcMessageEventHandler<IrcxMessage> Ircx;

        /// <summary>
        ///   Raises the Ircx event.
        /// </summary>
        protected internal void OnIrcx(IrcMessageEventArgs<IrcxMessage> e)
        {
            if (this.Ircx != null)
            {
                this.Ircx(this, e);
            }
        }

        /// <summary>
        ///   Occurs when a <see cref="IsIrcxMessage" /> is received.
        /// </summary>
        public event IrcMessageEventHandler<IsIrcxMessage> IsIrcx;

        /// <summary>
        ///   Raises the IsIrcx event.
        /// </summary>
        protected internal void OnIsIrcx(IrcMessageEventArgs<IsIrcxMessage> e)
        {
            if (this.IsIrcx != null)
            {
                this.IsIrcx(this, e);
            }
        }

        /// <summary>
        ///   Occurs when a <see cref="IrcxReplyMessage" /> is received.
        /// </summary>
        public event IrcMessageEventHandler<IrcxReplyMessage> IrcxReply;

        /// <summary>
        ///   Raises the IrcxReply event.
        /// </summary>
        protected internal void OnIrcxReply(IrcMessageEventArgs<IrcxReplyMessage> e)
        {
            if (this.IrcxReply != null)
            {
                this.IrcxReply(this, e);
            }
        }

        /// <summary>
        ///   Occurs when a <see cref="KnockMessage" /> is received.
        /// </summary>
        public event IrcMessageEventHandler<KnockMessage> Knock;

        /// <summary>
        ///   Raises the Knock event.
        /// </summary>
        protected internal void OnKnock(IrcMessageEventArgs<KnockMessage> e)
        {
            if (this.Knock != null)
            {
                this.Knock(this, e);
            }
        }

        /// <summary>
        ///   Occurs when a <see cref="KnockReplyMessage" /> is received.
        /// </summary>
        public event IrcMessageEventHandler<KnockReplyMessage> KnockReply;

        /// <summary>
        ///   Raises the KnockReply event.
        /// </summary>
        protected internal void OnKnockReply(IrcMessageEventArgs<KnockReplyMessage> e)
        {
            if (this.KnockReply != null)
            {
                this.KnockReply(this, e);
            }
        }

        /// <summary>
        ///   Occurs when a <see cref="KnockRequestMessage" /> is received.
        /// </summary>
        public event IrcMessageEventHandler<KnockRequestMessage> KnockRequest;

        /// <summary>
        ///   Raises the KnockRequest event.
        /// </summary>
        protected internal void OnKnockRequest(IrcMessageEventArgs<KnockRequestMessage> e)
        {
            if (this.KnockRequest != null)
            {
                this.KnockRequest(this, e);
            }
        }

        /// <summary>
        ///   Occurs when a <see cref="WhisperMessage" /> is received.
        /// </summary>
        public event IrcMessageEventHandler<WhisperMessage> Whisper;

        /// <summary>
        ///   Raises the Whisper event.
        /// </summary>
        protected internal void OnWhisper(IrcMessageEventArgs<WhisperMessage> e)
        {
            if (this.Whisper != null)
            {
                this.Whisper(this, e);
            }
        }

        #endregion


        #region Watch

        /// <summary>
        ///   Occurs when a <see cref="WatchListClearMessage" /> is received.
        /// </summary>
        public event IrcMessageEventHandler<WatchListClearMessage> WatchListClear;

        /// <summary>
        ///   Raises the WatchListClear event.
        /// </summary>
        protected internal void OnWatchListClear(IrcMessageEventArgs<WatchListClearMessage> e)
        {
            if (this.WatchListClear != null)
            {
                this.WatchListClear(this, e);
            }
        }

        /// <summary>
        ///   Occurs when a <see cref="WatchListEditorMessage" /> is received.
        /// </summary>
        public event IrcMessageEventHandler<WatchListEditorMessage> WatchListEditor;

        /// <summary>
        ///   Raises the WatchListEditor event.
        /// </summary>
        protected internal void OnWatchListEditor(IrcMessageEventArgs<WatchListEditorMessage> e)
        {
            if (this.WatchListEditor != null)
            {
                this.WatchListEditor(this, e);
            }
        }

        /// <summary>
        ///   Occurs when a <see cref="WatchListRequestMessage" /> is received.
        /// </summary>
        public event IrcMessageEventHandler<WatchListRequestMessage> WatchListRequest;

        /// <summary>
        ///   Raises the WatchListRequest event.
        /// </summary>
        protected internal void OnWatchListRequest(IrcMessageEventArgs<WatchListRequestMessage> e)
        {
            if (this.WatchListRequest != null)
            {
                this.WatchListRequest(this, e);
            }
        }

        /// <summary>
        ///   Occurs when a <see cref="WatchStatusRequestMessage" /> is received.
        /// </summary>
        public event IrcMessageEventHandler<WatchStatusRequestMessage> WatchStatusRequest;

        /// <summary>
        ///   Raises the WatchStatusRequest event.
        /// </summary>
        protected internal void OnWatchStatusRequest(IrcMessageEventArgs<WatchStatusRequestMessage> e)
        {
            if (this.WatchStatusRequest != null)
            {
                this.WatchStatusRequest(this, e);
            }
        }

        /// <summary>
        ///   Occurs when a <see cref="WatchListEndReplyMessage" /> is received.
        /// </summary>
        public event IrcMessageEventHandler<WatchListEndReplyMessage> WatchListEndReply;

        /// <summary>
        ///   Raises the WatchListEndReply event.
        /// </summary>
        protected internal void OnWatchListEndReply(IrcMessageEventArgs<WatchListEndReplyMessage> e)
        {
            if (this.WatchListEndReply != null)
            {
                this.WatchListEndReply(this, e);
            }
        }

        /// <summary>
        ///   Occurs when a <see cref="WatchStatusReplyMessage" /> is received.
        /// </summary>
        public event IrcMessageEventHandler<WatchStatusReplyMessage> WatchStatusReply;

        /// <summary>
        ///   Raises the WatchStatusReply event.
        /// </summary>
        protected internal void OnWatchStatusReply(IrcMessageEventArgs<WatchStatusReplyMessage> e)
        {
            if (this.WatchStatusReply != null)
            {
                this.WatchStatusReply(this, e);
            }
        }

        /// <summary>
        ///   Occurs when a <see cref="WatchStatusNicksReplyMessage" /> is received.
        /// </summary>
        public event IrcMessageEventHandler<WatchStatusNicksReplyMessage> WatchStatusNicksReply;

        /// <summary>
        ///   Raises the <see cref="WatchStatusNicksReply" /> event.
        /// </summary>
        protected internal void OnWatchStatusNicksReply(IrcMessageEventArgs<WatchStatusNicksReplyMessage> ircMessageEventArgs)
        {
            if (this.WatchStatusNicksReply != null)
            {
                this.WatchStatusNicksReply(this, ircMessageEventArgs);
            }
        }

        /// <summary>
        ///   Occurs when a <see cref="WatchStoppedMessage" /> is received.
        /// </summary>
        public event IrcMessageEventHandler<WatchStoppedMessage> WatchStopped;

        /// <summary>
        ///   Raises the WatchStoppedReply event.
        /// </summary>
        protected internal void OnWatchStopped(IrcMessageEventArgs<WatchStoppedMessage> e)
        {
            if (this.WatchStopped != null)
            {
                this.WatchStopped(this, e);
            }
        }

        /// <summary>
        ///   Occurs when a <see cref="WatchedUserOnlineMessage" /> is received.
        /// </summary>
        public event IrcMessageEventHandler<WatchedUserOnlineMessage> WatchedUserOnline;

        /// <summary>
        ///   Raises the <see cref="WatchedUserOnline" /> event.
        /// </summary>
        protected internal void OnWatchedUserOnline(IrcMessageEventArgs<WatchedUserOnlineMessage> ircMessageEventArgs)
        {
            if (this.WatchedUserOnline != null)
            {
                this.WatchedUserOnline(this, ircMessageEventArgs);
            }
        }

        /// <summary>
        ///   Occurs when a <see cref="WatchedUserOfflineMessage" /> is received.
        /// </summary>
        public event IrcMessageEventHandler<WatchedUserOfflineMessage> WatchedUserOffline;

        /// <summary>
        ///   Raises the <see cref="WatchedUserOffline" /> event.
        /// </summary>
        protected internal void OnWatchedUserOffline(IrcMessageEventArgs<WatchedUserOfflineMessage> ircMessageEventArgs)
        {
            if (this.WatchedUserOffline != null)
            {
                this.WatchedUserOffline(this, ircMessageEventArgs);
            }
        }

        #endregion


        #region Monitor

        /// <summary>
        ///   Occurs when a <see cref="MonitorListClearMessage" /> is received.
        /// </summary>
        public event IrcMessageEventHandler<MonitorListClearMessage> MonitorListClear;

        /// <summary>
        ///   Raises the <see cref="MonitorListClear" /> event.
        /// </summary>
        protected internal void OnMonitorListClear(IrcMessageEventArgs<MonitorListClearMessage> ircMessageEventArgs)
        {
            if (this.MonitorListClear != null)
            {
                this.MonitorListClear(this, ircMessageEventArgs);
            }
        }

        /// <summary>
        ///   Occurs when a <see cref="MonitorListRequestMessage" /> is received.
        /// </summary>
        public event IrcMessageEventHandler<MonitorListRequestMessage> MonitorListRequest;

        /// <summary>
        ///   Raises the <see cref="MonitorListRequest" /> event.
        /// </summary>
        protected internal void OnMonitorListRequest(IrcMessageEventArgs<MonitorListRequestMessage> ircMessageEventArgs)
        {
            if (this.MonitorListRequest != null)
            {
                this.MonitorListRequest(this, ircMessageEventArgs);
            }
        }

        /// <summary>
        ///   Occurs when a <see cref="MonitorStatusRequestMessage" /> is received.
        /// </summary>
        public event IrcMessageEventHandler<MonitorStatusRequestMessage> MonitorStatusRequest;

        /// <summary>
        ///   Raises the <see cref="MonitorStatusRequest" /> event.
        /// </summary>
        protected internal void OnMonitorStatusRequest(IrcMessageEventArgs<MonitorStatusRequestMessage> ircMessageEventArgs)
        {
            if (this.MonitorStatusRequest != null)
            {
                this.MonitorStatusRequest(this, ircMessageEventArgs);
            }
        }

        /// <summary>
        ///   Occurs when a <see cref="MonitorAddUsersMessage" /> is received.
        /// </summary>
        public event IrcMessageEventHandler<MonitorAddUsersMessage> MonitorAddUsers;

        /// <summary>
        ///   Raises the <see cref="MonitorAddUsers" /> event.
        /// </summary>
        protected internal void OnMonitorAddUsers(IrcMessageEventArgs<MonitorAddUsersMessage> ircMessageEventArgs)
        {
            if (this.MonitorAddUsers != null)
            {
                this.MonitorAddUsers(this, ircMessageEventArgs);
            }
        }

        /// <summary>
        ///   Occurs when a <see cref="MonitorRemoveUsersMessage" /> is received.
        /// </summary>
        public event IrcMessageEventHandler<MonitorRemoveUsersMessage> MonitorRemoveUsers;

        /// <summary>
        ///   Raises the <see cref="MonitorRemoveUsers" /> event.
        /// </summary>
        protected internal void OnMonitorRemoveUsers(IrcMessageEventArgs<MonitorRemoveUsersMessage> ircMessageEventArgs)
        {
            if (this.MonitorRemoveUsers != null)
            {
                this.MonitorRemoveUsers(this, ircMessageEventArgs);
            }
        }

        /// <summary>
        ///   Occurs when a <see cref="MonitoredUserOnlineMessage" /> is received.
        /// </summary>
        public event IrcMessageEventHandler<MonitoredUserOnlineMessage> MonitoredUserOnline;

        /// <summary>
        ///   Raises the <see cref="MonitoredUserOnline" /> event.
        /// </summary>
        protected internal void OnMonitoredUserOnline(IrcMessageEventArgs<MonitoredUserOnlineMessage> ircMessageEventArgs)
        {
            if (this.MonitoredUserOnline != null)
            {
                this.MonitoredUserOnline(this, ircMessageEventArgs);
            }
        }

        /// <summary>
        ///   Occurs when a <see cref="MonitoredUserOfflineMessage" /> is received.
        /// </summary>
        public event IrcMessageEventHandler<MonitoredUserOfflineMessage> MonitoredUserOffline;

        /// <summary>
        ///   Raises the <see cref="MonitoredUserOffline" /> event.
        /// </summary>
        protected internal void OnMonitoredUserOffline(IrcMessageEventArgs<MonitoredUserOfflineMessage> ircMessageEventArgs)
        {
            if (this.MonitoredUserOffline != null)
            {
                this.MonitoredUserOffline(this, ircMessageEventArgs);
            }
        }

        /// <summary>
        ///   Occurs when a <see cref="MonitorListReplyMessage" /> is received.
        /// </summary>
        public event IrcMessageEventHandler<MonitorListReplyMessage> MonitorListReply;

        /// <summary>
        ///   Raises the <see cref="MonitorListReply" /> event.
        /// </summary>
        protected internal void OnMonitorListReply(IrcMessageEventArgs<MonitorListReplyMessage> ircMessageEventArgs)
        {
            if (this.MonitorListReply != null)
            {
                this.MonitorListReply(this, ircMessageEventArgs);
            }
        }

        /// <summary>
        ///   Occurs when a <see cref="MonitorListEndReplyMessage" /> is received.
        /// </summary>
        public event IrcMessageEventHandler<MonitorListEndReplyMessage> MonitorListEndReply;

        /// <summary>
        ///   Raises the <see cref="MonitorListEndReply" /> event.
        /// </summary>
        protected internal void OnMonitorListEndReply(IrcMessageEventArgs<MonitorListEndReplyMessage> ircMessageEventArgs)
        {
            if (this.MonitorListEndReply != null)
            {
                this.MonitorListEndReply(this, ircMessageEventArgs);
            }
        }

        /// <summary>
        ///   Occurs when a <see cref="MonitorListFullMessage" /> is received.
        /// </summary>
        public event IrcMessageEventHandler<MonitorListFullMessage> MonitorListFull;

        /// <summary>
        ///   Raises the <see cref="MonitorListFull" /> event.
        /// </summary>
        protected internal void OnMonitorListFull(IrcMessageEventArgs<MonitorListFullMessage> ircMessageEventArgs)
        {
            if (this.MonitorListFull != null)
            {
                this.MonitorListFull(this, ircMessageEventArgs);
            }
        }

        #endregion


        #region Channel Scoped Chat

        /// <summary>
        ///   Occurs when a <see cref="ChannelScopedChatMessage" /> is received.
        /// </summary>
        public event IrcMessageEventHandler<ChannelScopedChatMessage> ChannelScopedChat;

        /// <summary>
        ///   Raises the ChannelScopedChat event.
        /// </summary>
        protected internal void OnChannelScopedChat(IrcMessageEventArgs<ChannelScopedChatMessage> e)
        {
            if (this.ChannelScopedChat != null)
            {
                this.ChannelScopedChat(this, e);
            }
        }

        /// <summary>
        ///   Occurs when a <see cref="ChannelScopedNoticeMessage" /> is received.
        /// </summary>
        public event IrcMessageEventHandler<ChannelScopedNoticeMessage> ChannelScopedNotice;

        /// <summary>
        ///   Raises the ChannelScopedNotice event.
        /// </summary>
        protected internal void OnChannelScopedNotice(IrcMessageEventArgs<ChannelScopedNoticeMessage> e)
        {
            if (this.ChannelScopedNotice != null)
            {
                this.ChannelScopedNotice(this, e);
            }
        }

        #endregion
    }
}
