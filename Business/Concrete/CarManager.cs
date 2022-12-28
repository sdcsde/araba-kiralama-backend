using Business.Abstract;
using Business.BusinessAspects;
using Business.Constants;
using Business.ValidationRules.FluentValidation;
using Core.Aspects.Autofac.Caching;
using Core.Aspects.Autofac.Transaction;
using Core.Aspects.Autofac.Validation;
using Core.Utilities.Business;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using Entities.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Business.Concrete
{
    public class CarManager:ICarService
    {
        ICarDal _carDal;

        public CarManager(ICarDal carDal)
        {
            _carDal = carDal;
        }

        //[SecuredOperation("product.add,admin")]
        [ValidationAspect(typeof(CarValidator))]
        [CacheRemoveAspect("IProductService.Get")]
        public IResult Add(Car car)
        {
            //_carDal.Add(car);
            //return new SuccessResult(Messages.CarAdded); - eski versiyon

            IResult result = BusinessRules.Run(CheckIfCarCountOfBrand(car.BrandId),CheckIfCarNameExists(car.CarName));//yeni versiyon

            if (result != null)
            {
                return result;
            }

            _carDal.Add(car);

            return new SuccessResult(Messages.CarAdded);

        }

        [TransactionScopeAspect]
        public IResult AddTransactionalTest(Car car)
        {

            Add(car);
            if (car.DailyPrice < 100)
            {
                throw new Exception(" ");
            }
            Add(car);
            return null;
        }

        public IResult Delete(Car car)
        {
            _carDal.Delete(car);
            return new SuccessResult(Messages.CarDeleted);
        }

        [CacheAspect]
        public IDataResult<List<Car>> GetAll()
        {
            return new SuccessDataResult<List<Car>>(_carDal.GetAll(), Messages.CarsListed);
        }

        public IDataResult<List<Car>> GetAllByCarId(int id)
        {
            return new SuccessDataResult<List<Car>>(_carDal.GetAll(c=>c.CarId == id),Messages.CarInfo);
        }

        public IDataResult<Car> GetById(int id)
        {
            return new SuccessDataResult<Car>(_carDal.Get(c=>c.CarId == id),Messages.CarInfo);
        }

        public IDataResult<List<Car>> GetByUnitPrice(int minDailyPrice, int maxDailyPrice)
        {
            return new SuccessDataResult<List<Car>>(_carDal.GetAll(c => c.DailyPrice >= minDailyPrice && c.DailyPrice <= maxDailyPrice),Messages.PriceListed);
        }

        public IDataResult<List<CarDetailDto>> GetCarDetails()
        {
            return new SuccessDataResult<List<CarDetailDto>>(_carDal.GetCarDetails(),Messages.DetailsListed);
        }

        public IDataResult<List<Car>> GetCarsByBrandId(int id)
        {
            return new SuccessDataResult<List<Car>>(_carDal.GetAll(b => b.BrandId == id),Messages.CarsListed);
        }

        public IDataResult<List<Car>> GetCarsByColorId(int id)
        {
            return new SuccessDataResult<List<Car>>(_carDal.GetAll(c => c.ColorId == id),Messages.CarsListed);
        }

        public IResult Update(Car car)
        {
            _carDal.Update(car);
            return new SuccessResult(Messages.CarUpdated);
        }

        public IDataResult<List<CarFilterDto>> GetFilteredCars(int brandId, int colorId, int minDailyPrice, int maxDailyPrice)
        {
            return new SuccessDataResult<List<CarFilterDto>>(_carDal.GetFilteredCars(brandId, colorId, minDailyPrice, maxDailyPrice));
        }

        public IDataResult<List<Car>> GetCarDetailsByBrandIdAndColorId(int brandId, int colorId)
        {
            var resultByBrandId = this.GetCarsByBrandId(brandId);

            if (resultByBrandId.Success)
            {
                var result = resultByBrandId.Data.Where(c => c.ColorId == colorId).ToList();

                return new SuccessDataResult<List<Car>>(result);
            }

            return new ErrorDataResult<List<Car>>(resultByBrandId.Message);
        }

    private IResult CheckIfCarCountOfBrand(int brandId)
        {
            var result = _carDal.GetAll(c => c.BrandId == brandId).Count;
            if (result >= 15)
            {
                return new ErrorResult();
            }
            return new SuccessResult();
        }

        private IResult CheckIfCarNameExists(string productName)
        {
            var result = _carDal.GetAll(c => c.CarName == productName).Any();
            if (result) // any metodunu kullanmayıp : result != null şeklinde veya count kullanıp result > 0 yaparak da kullanılabilir.
            {
                return new ErrorResult(Messages.CarNameAlreadyExists);
            }
            return new SuccessResult();
        }

    }
}
