using System;
using System.Collections;
using System.Collections.Specialized;
using System.Globalization;
using System.IO;

namespace Supay.Irc.Messages {

  /// <summary>
  ///   Writes <see cref="IrcMessage"/> data to a <see cref="TextWriter"/> in IRC protocol format. </summary>
  public class IrcMessageWriter {

    private ArrayList _parameters = new ArrayList();
    private NameValueCollection _listParams = new NameValueCollection();
    private NameValueCollection _splitParams = new NameValueCollection();

    #region Constructors

    /// <summary>
    ///   Creates a new instance of the IrcMessageWriter class without an <see cref="InnerWriter"/> to write to. </summary>
    public IrcMessageWriter() {
      this.ResetDefaults();
    }

    /// <summary>
    ///   Creates a new instance of the IrcMessageWriter class with the given <see cref="InnerWriter"/> to write to. </summary>
    public IrcMessageWriter(TextWriter writer) {
      this.InnerWriter = writer;
      this.ResetDefaults();
    }

    #endregion

    #region Properties

    /// <summary>
    ///   Gets or sets the <see cref="TextWriter"/> which will be written to. </summary>
    public TextWriter InnerWriter {
      get;
      set;
    }

    /// <summary>
    ///   Gets or sets the ID of the sender of the message. </summary>
    public string Sender {
      get;
      set;
    }

    /// <summary>
    ///   Gets or sets if a new line is appended to the end of messages when they are written. </summary>
    public bool AppendNewLine {
      get;
      set;
    }

    #endregion

    #region Methods

    /// <summary>
    ///   Adds the given possibly-splittable parameter to the writer. </summary>
    /// <param name="value">
    ///   The parameter to add to the writer. </param>
    /// <param name="splittable">
    ///   Indicates if the parameter can be split across messages written. </param>
    public void AddParameter(string value, bool splittable) {
      _parameters.Add(value);
      if (splittable) {
        this.AddSplittableParameter();
      }
    }

    /// <summary>
    ///   Adds the given non-splittable parameter to the writer. </summary>
    /// <param name="value">
    ///   The parameter to add to the writer. </param>
    public void AddParameter(string value) {
      this.AddParameter(value, false);
    }

    /// <summary>
    ///   Adds a possibly-splittable list of parameters to the writer. </summary>
    /// <param name="value">
    ///   The list of parameters to add. </param>
    /// <param name="separator">
    ///   The seperator to write between values in the list. </param>
    /// <param name="splittable">
    ///   Indicates if the parameters can be split across messages written. </param>
    public void AddList(IList value, string separator, bool splittable) {
      _listParams[_parameters.Count.ToString(CultureInfo.InvariantCulture)] = separator;
      _parameters.Add(value);
      if (splittable) {
        this.AddSplittableParameter();
      }
    }

    /// <summary>
    ///   Adds a splittable list of parameters to the writer. </summary>
    /// <param name="value">
    ///   The list of parameters to add. </param>
    /// <param name="separator">
    ///   The seperator to write between values in the list. </param>
    public void AddList(IList value, string separator) {
      this.AddList(value, separator, true);
    }

    /// <summary>
    ///   Writes the current message data to the inner writer in irc protocol format. </summary>
    public void Write() {
      //TODO Implement message splitting on IrcMessageWriter.Write
      if (this.InnerWriter == null) {
        this.InnerWriter = new StringWriter(CultureInfo.InvariantCulture);
      }

      if (!string.IsNullOrEmpty(this.Sender)) {
        this.InnerWriter.Write(":");
        this.InnerWriter.Write(this.Sender);
        this.InnerWriter.Write(" ");
      }

      int paramCount = _parameters.Count;
      if (paramCount > 0) {
        for (int i = 0; i < paramCount - 1; i++) {
          this.InnerWriter.Write(this.GetParamValue(i));
          this.InnerWriter.Write(" ");
        }
        string lastParam = GetParamValue(paramCount - 1);
        if (lastParam.IndexOf(" ", StringComparison.Ordinal) > 0) {
          this.InnerWriter.Write(":");
        }
        this.InnerWriter.Write(lastParam);
      }
      if (this.AppendNewLine) {
        this.InnerWriter.Write(System.Environment.NewLine);
      }

      this.ResetDefaults();
    }

    #endregion

    #region Private Methods

    private void ResetDefaults() {
      this.AppendNewLine = true;
      this.Sender = null;
      _parameters.Clear();
      _listParams.Clear();
    }

    private void AddSplittableParameter() {
      _splitParams[_parameters.Count.ToString(CultureInfo.InvariantCulture)] = string.Empty;
    }

    private string GetParamValue(int index) {
      object parameters = _parameters[index];

      StringCollection paramCollection = parameters as StringCollection;
      if (paramCollection != null) {
        string seperator = _listParams[index.ToString(CultureInfo.InvariantCulture)];
        return MessageUtil.CreateList(paramCollection, seperator);
      }

      IList paramList = parameters as IList;
      if (paramList != null) {
        string seperator = _listParams[index.ToString(CultureInfo.InvariantCulture)];
        return MessageUtil.CreateList(paramList, seperator);
      }

      return parameters.ToString();
    }

    #endregion

  } //class IrcMessageWriter
} //namespace Supay.Irc.Messages