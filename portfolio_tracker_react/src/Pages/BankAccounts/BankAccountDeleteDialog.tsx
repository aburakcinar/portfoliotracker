
import React from "react";
import { deleteBankAccountApi, IBankAccountModel } from "../../Api";
import { TrashIcon } from "@heroicons/react/24/outline";
import { ConfirmDialog } from "primereact/confirmdialog";

export interface IBankAccountDeleteDialogProps {
    item: IBankAccountModel;
    onDelete?: (item: IBankAccountModel) => void;
}

export const BankAccountDeleteDialog: React.FC<IBankAccountDeleteDialogProps> = (props) => {

    const { item, onDelete } = props;
    const [visible, setVisible] = React.useState<boolean>(false);

    const onDeleteHandler = () => { setVisible(true); };

    const accept = async () => {

        await deleteBankAccountApi(item.id);

        if (onDelete) {
            onDelete(item);
        }
        setVisible(false);
    };

    const reject = () => {
        setVisible(false);
    };

    return <>
        <ConfirmDialog
            group="declarative"
            visible={visible}
            onHide={() => setVisible(false)}
            message="Are you sure you want to delete bank account?"
            header="Delete Bank Account?"
            icon="pi pi-exclamation-triangle"
            accept={accept}
            reject={reject}
        />

        <button title="Delete" onClick={onDeleteHandler}>
            <TrashIcon className="size-5 text-red-500 hover:text-red-300" />
        </button>
    </>;
};