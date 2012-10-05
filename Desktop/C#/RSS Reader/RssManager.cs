#region Using
using System.Windows.Forms;
using System;
using System.Xml;
using System.Collections.ObjectModel;
using PC;
#endregion
[Serializable]
public class RssManager : IDisposable
{
    #region Variables
    private string _url;
    private string _feedTitle;
    private string _feedDescription;
    private Collection<Rss.Items> _rssItems = new Collection<Rss.Items>();
    private bool _IsDisposed;
    #endregion

    #region Constructors
    public RssManager()
    {
        _url = string.Empty;
    }

  public RssManager(string feedUrl)
  {
    _url = feedUrl;
  }

  #endregion

  #region Properties  
  public string Url
  {
    get { return _url; }
    set { _url = value; }
  }
  public Collection<Rss.Items> RssItems
  {
    get { return _rssItems; }
  }
      
  public string FeedTitle
  {
    get { return _feedTitle; }
  }
      
  public string FeedDescription
  {
    get { return _feedDescription; }
  }

  #endregion

  #region Methods

  

  public Collection<Rss.Items> GetFeed()
  {
    if (String.IsNullOrEmpty(Url))
      throw new ArgumentException("Bir Rss adresi belirtmelisiniz");
  try
  {
      using (XmlReader reader = XmlReader.Create(Url))
      {
          XmlDocument xmlDoc = new XmlDocument();
          try
          {
              xmlDoc.Load(reader);
          }
          catch
          {
              MessageBox.Show("Baþvurulan adreste bir söz dizimi hatasý var.\nSite yöneticilerine haber verin.");
          }
          ParseDocElements(xmlDoc.SelectSingleNode("//channel"), "title", ref _feedTitle);
          ParseDocElements(xmlDoc.SelectSingleNode("//channel"), "description", ref _feedDescription);
          ParseRssItems(xmlDoc);
          return _rssItems;
      }
  }
  catch 
  {
      MessageBox.Show("Servis þu anda hizmet vermiyor");
      return _rssItems;
  }
  }

  private void ParseRssItems(XmlDocument xmlDoc)
  {
          _rssItems.Clear();
          XmlNodeList nodes = xmlDoc.SelectNodes("rss/channel/item");

          foreach (XmlNode node in nodes)
          {
              Rss.Items item = new Rss.Items();
              ParseDocElements(node, "title", ref item.Title);
              ParseDocElements(node, "description", ref item.Description);
              ParseDocElements(node, "link", ref item.Link);
              string date = null;
              ParseDocElements(node, "pubDate", ref date);
              DateTime.TryParse(date, out item.Date);
              _rssItems.Add(item);
          }
  }

  private void ParseDocElements(XmlNode parent, string xPath, ref string property)
  {
      try
      {
          XmlNode node = parent.SelectSingleNode(xPath);
          if (node != null)
              property = node.InnerText;
          else
              property = "Unresolvable";
      }
      catch { }
  }

  #endregion

  #region IDisposable Members  
  private void Dispose(bool disposing)
  {
    if (disposing && !_IsDisposed)
    {
      _rssItems.Clear();
      _url = null;
      _feedTitle = null;
      _feedDescription = null;
    }
    _IsDisposed = true;
  }

  public void Dispose()
  {
    Dispose(true);
    GC.SuppressFinalize(this);
  }
  #endregion
}