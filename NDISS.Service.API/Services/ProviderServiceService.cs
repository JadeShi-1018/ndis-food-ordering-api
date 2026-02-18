using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NDISS.Service.API.Common;
using NDISS.Service.API.Domain;
using NDISS.Service.API.DTOs.ProviderService;
using NDISS.Service.API.Repositories;
using NDIS.Shared.Common.Extensions;

namespace NDISS.Service.API.Services
{
    public class ProviderServiceService : IProviderServiceService
    {
        private readonly IGenericRepository<ProviderService> _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<ProviderServiceService> _logger;

        public ProviderServiceService(
            IGenericRepository<ProviderService> repository,
            IMapper mapper,
            ILogger<ProviderServiceService> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<ProviderServiceResponseDto> AddProviderServiceAsync(ProviderServiceCreateDto dto)
        {
            try
            {
                var serviceTypeExists = await _repository.GetDbSet<ServiceType>()
                    .AnyAsync(st => st.ServiceTypeId == dto.ServiceTypeId);

                if (!serviceTypeExists)
                    throw new ResourceNotFoundException($"ServiceType {dto.ServiceTypeId} not found.");

                var providerService = _mapper.Map<ProviderService>(dto);
                providerService.ProviderServiceId = GenerateId();

                await _repository.AddAsync(providerService);
                await _repository.SaveAsync();

                providerService.ServiceType = await _repository
                    .GetDbSet<ServiceType>()
                    .FirstOrDefaultAsync(st => st.ServiceTypeId == providerService.ServiceTypeId);

                return _mapper.Map<ProviderServiceResponseDto>(providerService);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while adding new ProviderService.");
                throw;
            }
        }

        private string GenerateId() => Guid.NewGuid().ToString();
    }
}
