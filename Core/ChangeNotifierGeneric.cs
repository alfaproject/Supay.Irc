using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq.Expressions;

namespace Supay.Core {
  public class ChangeNotifier<T> {

    private Func<PropertyChangedEventHandler> _notifier;
    private string _name;
    private T _value;

    public ChangeNotifier(Expression<Func<T>> expression, Func<PropertyChangedEventHandler> notifier) {
      if (expression.NodeType != ExpressionType.Lambda) {
        throw new ArgumentException("Value must be a lamda expression", "expression");
      } else if (!(expression.Body is MemberExpression)) {
        throw new ArgumentException("The body of the expression must be a memberref", "expression");
      } else {
        MemberExpression m = (MemberExpression)expression.Body;
        _name = m.Member.Name;
      }

      _notifier = notifier;
    }

    public ChangeNotifier(Expression<Func<T>> expression, Func<PropertyChangedEventHandler> notifier, T initialValue)
      : this(expression, notifier) {
      _value = initialValue;
    }
    
    public T Value {
      get {
        return _value;
      }
      set {
        if (!EqualityComparer<T>.Default.Equals(_value, value)) {
          _value = value;

          // Get the current list of registered event handlers
          // and invoke them with the correct 'sender' and event args
          PropertyChangedEventHandler handler = _notifier();
          if (handler != null) {
            handler(_notifier.Target, new PropertyChangedEventArgs(_name));
          }
        }
      }
    }

  } //class ChangeNotifier<T>
} //namespace Supay.Core