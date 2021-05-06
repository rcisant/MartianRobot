using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MartianRobot.Infrastructure.Models
{
    public class RequestsEntityTypeConfiguration : IEntityTypeConfiguration<MartianRobotData>
    {
        public void Configure(EntityTypeBuilder<MartianRobotData> builder)
        {

        }
    }
}
