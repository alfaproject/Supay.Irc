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
    ///   Initializes a new instance of the <see cref="IrcMessageWriter"/> class. </summary>
    public IrcMessageWriter() {
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

    public void Write(IrcMessage message) {
      //TODO Implement message splitting.

      if (message.Sender != null && !string.IsNullOrEmpty(message.Sender.Nickname)) {
        Write(":");
        Write(message.Sender.IrcMask);
        Write(" ");
      }

      // fill parameters list
      message.AddParametersToFormat(this);

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
    }

    public void WriteLine(IrcMessage message) {
      Write(message);
      WriteLine();
    }

    #endregion

    #region Private Methods

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