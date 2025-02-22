import React, { useEffect, useState } from "react";
import { getBankAccountApi, IBankAccountModel } from "../Api/BankAccount.api";

export const useBankAccount = (
  bankAccountId: string | null | undefined
): IBankAccountModel | null => {
  const [bankAccount, setBankAccount] = useState<IBankAccountModel | null>(
    null
  );

  useEffect(() => {
    if (bankAccountId) {
      getBankAccountApi(bankAccountId)
        .then((item) => {
          setBankAccount(item);
        })
        .catch((_) => setBankAccount(null));
    } else {
      setBankAccount(null);
    }
  }, [bankAccountId]);

  return bankAccount;
};
