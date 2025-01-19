import { CodeBracketIcon } from "@heroicons/react/24/outline";
import { Link, Outlet } from "react-router";

export const ExampleControls: React.FC = () => {
  return (
    <div>
      <h3 className="text-5xl text-green p-4">Example Controls</h3>

      <div className="flex w-full">
        <div className="grow-0 min-w-64 border-r-1 border-r-slate-300">
          <nav className="pt-16">
            <ul className="text-white ">
              <Link to="addinvestmentcontrol">
                <li className="p-2 hover:bg-highlight">
                  <div className="flex ">
                    <CodeBracketIcon className="size-9 flex-none px-2" />
                    <span className="grow py-1">Add Investment Control</span>
                  </div>
                </li>
              </Link>
              <Link to="addupdateholdingform">
                <li className="p-2 hover:bg-highlight">
                  <div className="flex">
                    <CodeBracketIcon className="size-9 flex-none px-2 " />
                    <span className="grow py-1">Add Update Holding Form</span>
                  </div>
                </li>
              </Link>
              <Link to="darkmodetoggle">
                <li className="p-2 hover:bg-highlight">
                  <div className="flex">
                    <CodeBracketIcon className="size-9 flex-none px-2 " />
                    <span className="grow py-1">Dark Mode Toggle</span>
                  </div>
                </li>
              </Link>
              <Link to="comboboxes">
                <li className="p-2 hover:bg-highlight">
                  <div className="flex">
                    <CodeBracketIcon className="size-9 flex-none px-2 " />
                    <span className="grow py-1">ComboBox</span>
                  </div>
                </li>
              </Link>
              <Link to="inputtextes">
                <li className="p-2 hover:bg-highlight">
                  <div className="flex">
                    <CodeBracketIcon className="size-9 flex-none px-2 " />
                    <span className="grow py-1">InputText</span>
                  </div>
                </li>
              </Link>
              <Link to="buttons">
                <li className="p-2 hover:bg-highlight">
                  <div className="flex">
                    <CodeBracketIcon className="size-9 flex-none px-2 " />
                    <span className="grow py-1">Buttons</span>
                  </div>
                </li>
              </Link>
            </ul>
          </nav>
        </div>
        <div className="grow">
          <Outlet />
        </div>
      </div>
    </div>
  );
};
