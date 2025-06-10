import api from "./Api";

export interface IApiHealthStatus {
  apiName: string;
  isHealthy: boolean;
  endpoint: string;
}

export interface IApiService {
  name: string;
  endpoint: string;
}

// List of all PortfolioTracker API services
export const apiServices: IApiService[] = [
  { name: "Asset API", endpoint: "/asset/health" },
  { name: "Bank Account API", endpoint: "/bankaccount/health" },
  { name: "Exchange API", endpoint: "/exchange/health" },
  { name: "Imports API", endpoint: "/import/health" },
  { name: "Portfolio API", endpoint: "/portfolio/health" },
  { name: "Transaction API", endpoint: "/transaction/health" },
];

export const checkApiHealthStatus = async (endpoint: string): Promise<boolean> => {
  try {
    const response = await api.get(endpoint);
    return response.status === 200;
  } catch (error) {
    return false;
  }
};

export const getAllApiHealthStatuses = async (): Promise<IApiHealthStatus[]> => {
  const healthStatuses = await Promise.all(
    apiServices.map(async (service) => {
      const isHealthy = await checkApiHealthStatus(service.endpoint);
      return {
        apiName: service.name,
        isHealthy,
        endpoint: service.endpoint
      };
    })
  );

  return healthStatuses;
};
