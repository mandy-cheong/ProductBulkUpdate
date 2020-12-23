using System.Collections.Generic;
using System.Data;

public interface IBrandShippingService
{
    bool AddBrandShipping(BrandShipping brandShipping);
    bool UpdateBrandShipping(BrandShipping brandShipping);
    DataTable GetBrandShippings(BrandShipping brandShipping);

}