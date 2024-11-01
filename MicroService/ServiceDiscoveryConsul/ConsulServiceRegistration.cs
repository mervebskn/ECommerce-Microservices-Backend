using Consul;

namespace ServiceDiscoveryConsul
{
    public class ConsulServiceRegistration
    {
        private readonly string _serviceId;
        private readonly string _serviceName;
        private readonly int _servicePort;

        public ConsulServiceRegistration(string serviceId, string serviceName, int servicePort)
        {
            _serviceId = serviceId;
            _serviceName = serviceName;
            _servicePort = servicePort;
        }

        public async Task RegisterService()
        {
            using (var client = new ConsulClient())
            {
                var registration = new AgentServiceRegistration()
                {
                    ID = _serviceId,
                    Name = _serviceName, 
                    Address = "localhost", // Docker kullanıyorsanız, burayı Docker konteynerinin IP'sine göre değiştirin
                    Port = _servicePort,
                    Tags = new[] { "api" } // optional
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
