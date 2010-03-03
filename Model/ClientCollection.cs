using System;
using System.Collections.ObjectModel;

namespace Supay.Irc {

  /// <summary>
  ///     <para>
  ///       A collection that stores <see cref='Supay.Irc.Client'/> objects.
  ///    </para>
  /// </summary>
  /// <seealso cref='Supay.Irc.ClientCollection'/>
  [Serializable]
  public class ClientCollection : ObservableCollection<Client> {

    //public Client FindClient( string serverName )
    //{
    //    foreach ( Client c in this )
    //    {
    //        if ( c.ServerName == name )
    //        {
    //            return c;
    //        }
    //    }
    //    return null;
    //}

  }

}