using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LiteDB;

public class StructDB
{
    [BsonId]
    public Guid ID { get; set; }
    public DateTime dtHostPing { get; set; }
    public string TypePing { get; set; }
    public string MessageText { get; set; }

}
class MyLiteDb
{
    private string liteDbpath;

    public MyLiteDb(string databasePath)
    {
        this.liteDbpath = databasePath;
    }

    public List<string> GetTypesPing()      //List Состояния которое возвращает пинг
    {
        var PingTypes = new List<string>();
        PingTypes.Add("BadDestination");
        PingTypes.Add("BadHeader");
        PingTypes.Add("BadOption");
        PingTypes.Add("BadRoute");
        PingTypes.Add("DestinationHostUnreachable");
        PingTypes.Add("DestinationNetworkUnreachable");
        PingTypes.Add("DestinationPortUnreachable");
        PingTypes.Add("DestinationProhibited");
        PingTypes.Add("DestinationProtocolUnreachable");
        PingTypes.Add("DestinationScopeMismatch");
        PingTypes.Add("DestinationUnreachable");
        PingTypes.Add("HardwareError");
        PingTypes.Add("IcmpError");
        PingTypes.Add("NoResources");
        PingTypes.Add("PacketTooBig");
        PingTypes.Add("ParameterProblem");
        PingTypes.Add("SourceQuench");
        PingTypes.Add("Success");
        PingTypes.Add("TimedOut");
        PingTypes.Add("TimeExceeded");
        PingTypes.Add("TtlExpired");
        PingTypes.Add("TtlReassemblyTimeExceeded");
        PingTypes.Add("Unknown");
        PingTypes.Add("UnrecognizedNextHeader");
        return PingTypes;
    }

    public IList<StructDB> Get(string TypeSelect, DateTime dtSelect)
    {
        var ReZult = new List<StructDB>();

        using (var db = new LiteEngine(liteDbpath))
        {
            var type = db.GetCollection<StructDB>("PingHost");
            IEnumerable<StructDB> filter;

            if (TypeSelect.Equals("All"))
            {
                filter = type.All();

            }
            else
            {
                filter = type.Find(i => i.TypePing.Equals(TypeSelect));
            }

            foreach (StructDB item in filter)
            {
                ReZult.Add(item);
            }

            return ReZult.FindAll(i => i.dtHostPing.Date == dtSelect.Date);
        }
    }

    public IList<StructDB> GetAll()
    {
        var rezultToReturn = new List<StructDB>();

        using (var db = new LiteEngine(liteDbpath))
        {
            var pingHost = db.GetCollection<StructDB>("PingHost");

            var results = pingHost.All();

            foreach (StructDB item in results)
            {
                rezultToReturn.Add(item);
            }

            return rezultToReturn;
        }
    }

    public void Add(StructDB dBHost)
    {
        using (var db = new LiteEngine(liteDbpath))
        {
            var Hostcollect = db.GetCollection<StructDB>("PingHost");
            Hostcollect.Insert(dBHost);
            IndexIsHost(Hostcollect);

        }
    }

    private void IndexIsHost(Collection<StructDB> hostcollect)
    {
        hostcollect.EnsureIndex(x => x.ID);
        hostcollect.EnsureIndex(x => x.MessageText);
        hostcollect.EnsureIndex(x => x.dtHostPing);
        hostcollect.EnsureIndex(x=>x.TypePing);
    }
}

