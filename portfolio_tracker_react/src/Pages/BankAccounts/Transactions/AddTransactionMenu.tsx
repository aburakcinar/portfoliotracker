import React from "react";
import { ChevronDownIcon } from '@heroicons/react/24/outline'
import { MenuItem } from "primereact/menuitem";
import { SplitButton } from "primereact/splitbutton";
import { Button } from "primereact/button";
import { useNavigate } from "react-router";


export interface IAddTransactionMenuProps {
    bankAccountId: string;
    currencyCode: string;
}

export const AddTransactionMenu: React.FC<IAddTransactionMenuProps> = (props) => {

    const { bankAccountId, currencyCode } = props;
    const navigate = useNavigate();

    const addNewIcon = () => {

        return <ChevronDownIcon className="size-4" />
    };

    const items: MenuItem[] = [
        {
            label: 'Deposit',
            icon: 'pi pi-bolt',
            command: () => {

            }
        },
        {
            label: 'Withdraw',
            icon: 'pi pi-bolt',
            command: () => {

            }
        }
    ];

    const save = () => {

    }

    const goToImportForm = () => {
        navigate(`/bankaccounts/${bankAccountId}/transactions/import`);
    }


    return <div className="flex justify-end">
        <Button className="border border-r-1 border-r-black" label="Import" onClick={goToImportForm} />
        <SplitButton label="Add New" onClick={save} model={items} />
    </div>
};
