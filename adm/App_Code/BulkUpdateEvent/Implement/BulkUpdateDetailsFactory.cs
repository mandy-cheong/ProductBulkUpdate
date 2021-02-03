/// <summary>
/// Summary description for BulkUpdateDetailsFactory
/// </summary>
public class BulkUpdateDetailsFactory
{
    public BulkUpdateDetailsFactory()
    {
        //
        // TODO: Add constructor logic here
        //
    } 

    public  IBulkUpdateDetailsService CreateService(int type)
    {
        switch (type)
        {
            case (int)BulkUpdateType.商品上下架時間:
                return new ProductDateService();
            case (int)BulkUpdateType.活動標題:
                return new ProductEventService();
            case (int)BulkUpdateType.商品下架:
                return new ProductStatusService();
            case (int)BulkUpdateType.預購:
                return new ProductPreOrderService();
            case (int)BulkUpdateType.品牌館權重調整:
                return new ProductSortService();
            default:
                return null;


        }
    }
}