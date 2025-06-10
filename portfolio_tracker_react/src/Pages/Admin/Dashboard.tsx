import React, { useEffect, useState } from "react";
import { Card } from "primereact/card";
import { DataTable } from "primereact/datatable";
import { Column } from "primereact/column";
import { Tag } from "primereact/tag";
import { ProgressSpinner } from "primereact/progressspinner";
import { Button } from "primereact/button";
import { IApiHealthStatus, apiServices, getAllApiHealthStatuses } from "../../Api/Admin.api";
import { classNames } from "primereact/utils";

export const Dashboard: React.FC = () => {
  const [healthStatuses, setHealthStatuses] = useState<IApiHealthStatus[]>([]);
  const [loading, setLoading] = useState<boolean>(true);
  const [refreshing, setRefreshing] = useState<boolean>(false);

  const fetchHealthStatuses = async () => {
    try {
      setRefreshing(true);
      const statuses = await getAllApiHealthStatuses();
      setHealthStatuses(statuses);
    } catch (error) {
      console.error("Failed to fetch API health statuses:", error);
    } finally {
      setLoading(false);
      setRefreshing(false);
    }
  };

  useEffect(() => {
    fetchHealthStatuses();
  }, []);

  const refreshHealthStatuses = () => {
    fetchHealthStatuses();
  };

  const statusTemplate = (rowData: IApiHealthStatus) => {
    const classes = classNames(
      'font-bold',
      {'text-green-400':rowData.isHealthy}, 
      {'text-red-500':!rowData.isHealthy}) ;
    const text = rowData.isHealthy ? "Healthy" : "Unhealthy";

    return (
      <span className={classes}>{text}</span>
    );
  };

  const header = (
    <div className="flex justify-between items-center">
      <h2 className="text-xl font-bold">API Health Status</h2>
      <Button
        icon="pi pi-refresh"
        onClick={refreshHealthStatuses}
        disabled={refreshing}
        tooltip="Refresh health statuses"
        className="p-button-rounded p-button-outlined"
      />
    </div>
  );

  return (
    <div className="p-4">
      <h1 className="text-2xl font-bold mb-4">Admin Dashboard</h1>

      <Card className="mb-4">
        <div className="mb-4">
          <p className="text-gray-600">
            This dashboard displays the health status of all PortfolioTracker API services.
            Green indicates a healthy service, while red indicates a service that is currently unavailable.
          </p>
        </div>

        {loading ? (
          <div className="flex justify-center items-center p-6">
            <ProgressSpinner style={{ width: '50px', height: '50px' }} />
          </div>
        ) : (
          <DataTable
            value={healthStatuses}
            header={header}
            emptyMessage="No API services found"
            className="p-datatable-sm"
            stripedRows
          >
            <Column field="apiName" header="API Name" sortable />
            <Column field="endpoint" header="Endpoint" />
            <Column field="isHealthy" header="Status" body={statusTemplate} sortable />
          </DataTable>
        )}
      </Card>

      <Card className="mb-4">
        <h2 className="text-xl font-bold mb-3">API Services</h2>
        <p className="text-gray-600 mb-2">
          Total Services: {apiServices.length}
        </p>
        <p className="text-gray-600 mb-2">
          Healthy Services: {healthStatuses.filter(status => status.isHealthy).length}
        </p>
        <p className="text-gray-600">
          Unhealthy Services: {healthStatuses.filter(status => !status.isHealthy).length}
        </p>
      </Card>
    </div>
  );
};
