using Core.Utilities.Results;
using Entities.Concrete;
using Entities.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Abstract
{
    public interface ICarService
    {
        IDataResult<List<Car>> GetAll();
        IDataResult<Car> GetById(int id);
        IDataResult<List<Car>> GetAllByCarId(int id);
        IDataResult<List<Car>> GetByUnitPrice(int minDailyPrice, int maxDailyPrice);
        IDataResult<List<Car>> GetCarsByBrandId(int id);
        IDataResult<List<Car>> GetCarsByColorId(int id);
        IDataResult<List<CarDetailDto>> GetCarDetails();
        IResult Add(Car car);
        IResult Delete(Car car);
        IResult Update(Car car);
        public IResult AddTransactionalTest(Car car);

        IDataResult<List<CarFilterDto>> GetFilteredCars(int brandId, int colorId, int minDailyPrice, int maxDailyPrice);

        IDataResult<List<Car>> GetCarDetailsByBrandIdAndColorId(int brandId, int colorId);
    }
}
