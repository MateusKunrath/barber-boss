using BarberBoss.Domain.Entities;
using BarberBoss.Domain.Repositories.Billings;
using Moq;

namespace CommonTestUtilities.Repositories;

public class BillingsReadOnlyRepositoryBuilder
{
    private readonly Mock<IBillingsReadOnlyRepository> _repository;

    public BillingsReadOnlyRepositoryBuilder()
    {
        _repository = new Mock<IBillingsReadOnlyRepository>();
    }

    public BillingsReadOnlyRepositoryBuilder GetAllFiltered(List<Billing> billings)
    {
        _repository.Setup(repository => repository.GetAllFiltered(It.IsAny<GetBillingsFilters>())).ReturnsAsync((billings, billings.Count));
        return this;
    }

    public IBillingsReadOnlyRepository Build()
    {
        return _repository.Object;
    }
}