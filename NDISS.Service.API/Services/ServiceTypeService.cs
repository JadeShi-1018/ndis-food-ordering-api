using AutoMapper;
using NDISS.Service.API.Domain;
using NDISS.Service.API.Repositories;
using Microsoft.Extensions.Logging;
using NDIS.Shared.Common.Extensions;
using NDISS.Service.API.DTOs.ServiceType;

namespace NDISS.Service.API.Services
{
    public class ServiceTypeService : IServiceTypeService
    {
        private readonly IGenericRepository<ServiceType> _repository;
        private readonly ILogger<ServiceTypeService> _logger;
        private readonly IMapper _mapper;

        public ServiceTypeService(IGenericRepository<ServiceType> repository, ILogger<ServiceTypeService> logger, IMapper mapper)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ServiceTypeResponseDto>> GetAllServiceTypesAsync()
        {
            try
            {
                var serviceTypes = await _repository.GetAllAsync();
                return _mapper.Map<IEnumerable<ServiceTypeResponseDto>>(serviceTypes);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting all ServiceTypes.");
                throw;
            }
        }

        public async Task<ServiceTypeResponseDto?> GetServiceTypeByIdAsync(string id)
        {
            try
            {
                var serviceType = await _repository.GetByIdAsync(id);
                if (serviceType == null)
                    throw new ResourceNotFoundException($"ServiceType with id {id} not found.");

                return _mapper.Map<ServiceTypeResponseDto>(serviceType);
            }
            catch (ResourceNotFoundException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting ServiceType with id {ServiceTypeId}.", id);
                throw;
            }
        }

        public async Task<ServiceTypeResponseDto> AddServiceTypeAsync(ServiceTypeCreateDto dto)
        {
            try
            {
                var serviceType = _mapper.Map<ServiceType>(dto);
                serviceType.ServiceTypeId = Guid.NewGuid().ToString();

                await _repository.AddAsync(serviceType);
                await _repository.SaveAsync();

                return _mapper.Map<ServiceTypeResponseDto>(serviceType);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while adding new ServiceType.");
                throw;
            }
        }

        public async Task<ServiceTypeResponseDto> UpdateServiceTypeAsync(string id, ServiceTypeUpdateDto dto)
        {
            try
            {
                var serviceType = await _repository.GetByIdAsync(id);
                if (serviceType == null)
                    throw new ResourceNotFoundException($"ServiceType with id {id} not found.");

                _mapper.Map(dto, serviceType);

                await _repository.Update(serviceType);
                await _repository.SaveAsync();

                return _mapper.Map<ServiceTypeResponseDto>(serviceType);
            }
            catch (ResourceNotFoundException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating ServiceType with id {ServiceTypeId}.", id);
                throw;
            }
        }

        public async Task DeleteServiceTypeAsync(string id)
        {
            try
            {
                var serviceType = await _repository.GetByIdAsync(id);
                if (serviceType == null)
                    throw new ResourceNotFoundException($"ServiceType with id {id} not found.");

                await _repository.DeleteAsync(id);
                await _repository.SaveAsync();
            }
            catch (ResourceNotFoundException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting ServiceType with id {ServiceTypeId}.", id);
                throw;
            }
        }
    }
}
