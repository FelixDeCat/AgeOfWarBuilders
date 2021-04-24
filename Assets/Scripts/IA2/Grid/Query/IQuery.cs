using System.Collections.Generic;

public interface IQuery {

    IEnumerable<IGridEntity> Query();

}
