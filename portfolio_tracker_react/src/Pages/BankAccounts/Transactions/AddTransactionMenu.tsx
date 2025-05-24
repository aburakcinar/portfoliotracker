import React from "react";
import { ChevronDownIcon } from '@heroicons/react/24/outline'
import { MenuItem } from "primereact/menuitem";
import { SplitButton } from "primereact/splitbutton";


export interface IAddTransactionMenuProps {
    bankAccountId: string;
    currencyCode: string;
}

export const AddTransactionMenu: React.FC<IAddTransactionMenuProps> = (props) => {

    const { bankAccountId, currencyCode } = props;

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


    return <div className="flex justify-end">
        <SplitButton label="Add New" onClick={save} model={items} />
    </div>
};
