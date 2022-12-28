using Core.DataAccess.EntityFramework;
using DataAccess.Abstract;
using Entities.Concrete;
using Entities.DTOs;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Concrete.EntityFramework
{
    public class EfCarDal : EfEntityRepositoryBase<Car, RentACarDbContext>, ICarDal
    {
        public List<CarDetailDto> GetCarDetails()
        {
            using (RentACarDbContext context = new RentACarDbContext())
            {
                var result = from c in context.Cars
                             join b in context.Brands
                             on c.BrandId equals b.BrandId
                             join co in context.Colors
                             on c.ColorId equals co.ColorId

                             select new CarDetailDto
                             {
                                 CarId = c.CarId,
                                 CarName = c.CarName,
                                 BrandId = b.BrandId,
                                 ColorId = c.ColorId,
                                 BrandName = b.BrandName,
                                 ColorName = co.ColorName,
                                 ModelYear = c.ModelYear,
                                 DailyPrice = c.DailyPrice,
                                 Description = c.Description
                             };
                return result.ToList();
            }
        }

        public List<CarFilterDto> GetFilteredCars(int brandId, int colorId, int minDailyPrice, int maxDailyPrice)
        {
            using (RentACarDbContext context = new RentACarDbContext())
            {
                var result = from car in context.Cars
                             join color in context.Colors
                             on car.ColorId equals color.ColorId
                             join brand in context.Brands
                             on car.BrandId equals brand.BrandId
                             where car.BrandId == brandId
                             && car.ColorId == colorId
                             && car.DailyPrice >= minDailyPrice
                             && car.DailyPrice <= maxDailyPrice
                             select new CarFilterDto
                             {
                                 CarName = car.CarName,
                                 BrandName = brand.BrandName,
                                 ColorName = color.ColorName,
                                 DailyPrice = car.DailyPrice,
                                 ModelYear = car.ModelYear

                             };
                return result.ToList();

            }
        }
    }
}
