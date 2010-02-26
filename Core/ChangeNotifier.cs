using System;
using System.ComponentModel;
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

  } //class ChangeNotifier
} //namespace Supay.Core