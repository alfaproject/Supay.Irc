using System;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;

namespace Supay.Core {
  public class ChangeNotifier {

    private Func<PropertyChangedEventHandler> _notifier;

    public ChangeNotifier(Func<PropertyChangedEventHandler> notifier) {
      _notifier = notifier;
    }

    public ChangeNotifier<T> Create<T>(Expression<Func<T>> expression) {
      return new ChangeNotifier<T>(expression, _notifier);
    }

    public ChangeNotifier<T> Create<T>(Expression<Func<T>> expression, T initialValue) {
      return new ChangeNotifier<T>(expression, _notifier, initialValue);
    }

    public void CreateDependent<T>(Expression<Func<T>> property, params Expression<Func<object>>[] dependents) {
      // The name of the property which is dependent on the value of other properties
      string name = GetPropertyName(property);

      // The names of the other properties
      string[] dependentNames = dependents.Select<Expression, string>(GetPropertyName).ToArray();

      INotifyPropertyChanged sender = (INotifyPropertyChanged)_notifier.Target;
      sender.PropertyChanged += (o, e) => {
        // If one of our dependents changes, emit a PropertyChanged notification for our property
        if (dependentNames.Contains(e.PropertyName)) {
          PropertyChangedEventHandler h = _notifier();
          if (h != null) {
            h(o, new PropertyChangedEventArgs(name));
          }
        }
      };
    }

    private static string GetPropertyName(Expression expression) {
      while (!(expression is MemberExpression)) {
        if (expression is LambdaExpression) {
          expression = ((LambdaExpression)expression).Body;
        } else if (expression is UnaryExpression) {
          expression = ((UnaryExpression)expression).Operand;
        }
      }
      return ((MemberExpression)expression).Member.Name;
    }

  } //class ChangeNotifier
} //namespace Supay.Core