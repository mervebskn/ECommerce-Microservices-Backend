using Consul;

namespace ServiceDiscoveryConsul
{
    public class ConsulServiceRegistration
    {
        private readonly string _serviceId;
        private readonly string _serviceName;
        private readonly int _servicePort;
        private readonly string _serviceAddress;

        public ConsulServiceRegistration(string serviceId, string serviceName, int servicePort, string serviceAddress)
        {
            _serviceId = serviceId;
            _serviceName = serviceName;
            _servicePort = servicePort;
            _serviceAddress = serviceAddress;
        }

        // Servis kaydı
        public async Task RegisterService()
        {
            using (var client = new ConsulClient())
            {
                var registration = new AgentServiceRegistration()
                {
                    ID = _serviceId,
                    Name = _serviceName,
                    Address = _serviceAddress,  // Docker konteynerinin veya host'un IP adresi
                    Port = _servicePort,
                    Tags = new[] { "api" }  // Opsiyonel
                };

                await client.Agent.ServiceRegister(registration);
            }
        }

        public async Task DeregisterService()
        {
            using (var client = new ConsulClient())
            {
                await client.Agent.ServiceDeregister(_serviceId);
            }
        }
    }
}
