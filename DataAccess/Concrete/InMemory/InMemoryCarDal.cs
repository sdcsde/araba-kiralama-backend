using DataAccess.Abstract;
using Entities.Concrete;
using Entities.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace DataAccess.Concrete.InMemory
{
    public class InMemoryCarDal : ICarDal
    {
        List<Car> _car;

        public InMemoryCarDal()
        {
            _car = new List<Car>
            {
                new Car{ CarId = 1, BrandId = 1, ModelYear = 2022, ColorId = 128, DailyPrice = 600, Description = "Harder"},
                new Car{ CarId = 2, BrandId = 2, ModelYear = 2016, ColorId = 14, DailyPrice = 300, Description = "Better"},
                new Car{ CarId = 3, BrandId = 3, ModelYear = 1998, ColorId = 255, DailyPrice = 800, Description = "Faster"},
                new Car{ CarId = 4, BrandId = 4, ModelYear = 2020, ColorId = 90, DailyPrice = 1500, Description = "Stronger"}
            };
        }


        public void Add(Car car)
        {
            _car.Add(car);
        }

        public void Delete(Car car)
        {
            Car carToDelete = _car.SingleOrDefault(p => p.CarId == car.CarId);
            _car.Remove(carToDelete);
        }

        public Car Get(Expression<Func<Car, bool>> filter)
        {
            throw new NotImplementedException();
        }


        public List<Car> GetAll(Expression<Func<Car, bool>> filter = null)
        {
            throw new NotImplementedException();
        }

        public Car GetById(int id)
        {
            return _car.SingleOrDefault(p => p.CarId == id);
        }

        public List<CarDetailDto> GetCarDetails()
        {
            throw new NotImplementedException();
        }

        public List<CarFilterDto> GetFilteredCars(int brandId, int colorId, int minDailyPrice, int maxDailyPrice)
        {
            throw new NotImplementedException();
        }

        public void Update(Car car)
        {
            Car carToUpdate = _car.SingleOrDefault(p => p.CarId == car.CarId);
            carToUpdate.BrandId = car.BrandId;
            carToUpdate.ColorId = car.ColorId;
            carToUpdate.ModelYear = car.ModelYear;
            carToUpdate.DailyPrice = car.DailyPrice;
            carToUpdate.Description = car.Description;
        }
    }
}
