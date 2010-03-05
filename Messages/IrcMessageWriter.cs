using System;
using System.Collections;
using System.Collections.Specialized;
using System.Globalization;
using System.IO;

namespace Supay.Irc.Messages {

  /// <summary>
  ///   Writes <see cref="IrcMessage"/> data to a <see cref="TextWriter"/> in IRC protocol format. </summary>
  public class IrcMessageWriter : StringWriter {

    private readonly ArrayList _parameters = new ArrayList();
    private readonly NameValueCollection _listParams = new NameValueCollection();
    private readonly NameValueCollection _splitParams = new NameValueCollection();

    #region Constructor

    /// <summary>
    ///   Creates a new instance of the IrcMessageWriter class. </summary>
    public IrcMessageWriter() {
      resetDefaults();
    }

    #endregion

    #region Properties

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
    public void AddParameter(string value, bool splittable = false) {
      _parameters.Add(value);
      if (splittable) {
        addSplittableParameter();
      }
    }

    /// <summary>
    ///   Adds a possibly-splittable list of parameters to the writer. </summary>
    /// <param name="value">
    ///   The list of parameters to add. </param>
    /// <param name="separator">
    ///   The separator to write between values in the list. </param>
    /// <param name="splittable">
    ///   Indicates if the parameters can be split across messages written. </param>
    public void AddList(IList value, string separator, bool splittable = true) {
      _listParams[_parameters.Count.ToString(CultureInfo.InvariantCulture)] = separator;
      _parameters.Add(value);
      if (splittable) {
        addSplittableParameter();
      }
    }

    /// <summary>
    ///   Writes the current message data to the inner writer in IRC protocol format. </summary>
    public void Write() {
      //TODO Implement message splitting on IrcMessageWriter.Write

      if (!string.IsNullOrEmpty(Sender)) {
        Write(":");
        Write(Sender);
        Write(" ");
      }

      int paramCount = _parameters.Count;
      if (paramCount > 0) {
        for (int i = 0; i < paramCount - 1; i++) {
          Write(getParamValue(i));
          Write(" ");
        }
        string lastParam = getParamValue(paramCount - 1);
        if (lastParam.IndexOf(' ') > 0) {
          Write(":");
        }
        Write(lastParam);
      }
      if (AppendNewLine) {
        Write(Environment.NewLine);
      }

      resetDefaults();
    }

    #endregion

    #region Private Methods

    private void resetDefaults() {
      AppendNewLine = true;
      Sender = null;
      _parameters.Clear();
      _listParams.Clear();
    }

    private void addSplittableParameter() {
      _splitParams[_parameters.Count.ToString(CultureInfo.InvariantCulture)] = string.Empty;
    }

    private string getParamValue(int index) {
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