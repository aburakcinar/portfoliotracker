import classNames from "classnames";
import React, { useState, useRef, useEffect } from "react";

interface DropdownProps {
  label: string; // Label for the button that triggers the dropdown
  options: string[]; // Options to display in the dropdown
  onSelect: (option: string) => void; // Callback function to handle selection
  className?: string;
}

const Dropdown: React.FC<DropdownProps> = ({
  label,
  options,
  onSelect,
  className,
}) => {
  const [isOpen, setIsOpen] = useState(false);
  const dropdownRef = useRef<HTMLDivElement>(null);

  // Close the dropdown if the user clicks outside of it
  useEffect(() => {
    const handleClickOutside = (event: MouseEvent) => {
      if (
        dropdownRef.current &&
        !dropdownRef.current.contains(event.target as Node)
      ) {
        setIsOpen(false);
      }
    };
    document.addEventListener("mousedown", handleClickOutside);
    return () => document.removeEventListener("mousedown", handleClickOutside);
  }, []);

  const handleToggle = () => setIsOpen(!isOpen);

  const handleSelect = (option: string) => {
    onSelect(option);
    setIsOpen(false);
  };

  const mainClassName = classNames("relative inline-block text-left", {
    className: !!className,
  });

  return (
    <div className={mainClassName} ref={dropdownRef}>
      <button
        onClick={handleToggle}
        className="inline-flex w-full justify-center  border border-gray-300 bg-white p-2 text-sm font-medium text-gray-700 shadow-xs hover:bg-gray-50 focus:outline-hidden "
      >
        {label}
        <svg
          className={`ml-2 h-5 w-5 transform transition-transform ${
            isOpen ? "rotate-180" : "rotate-0"
          }`}
          xmlns="http://www.w3.org/2000/svg"
          viewBox="0 0 20 20"
          fill="currentColor"
        >
          <path
            fillRule="evenodd"
            d="M5.293 7.707a1 1 0 011.414 0L10 11.586l3.293-3.879a1 1 0 111.414 1.414l-4 4.5a1 1 0 01-1.414 0l-4-4.5a1 1 0 010-1.414z"
            clipRule="evenodd"
          />
        </svg>
      </button>

      {isOpen && (
        <div className="absolute left-0 mt-2 w-56 origin-top-right  bg-white shadow-lg ring-1 ring-black ring-opacity-5">
          <div className="py-1">
            {options.map((option, index) => (
              <button
                key={index}
                onClick={() => handleSelect(option)}
                className="block w-full px-4 py-2 text-left text-sm text-gray-700 hover:bg-gray-100 hover:text-gray-900"
              >
                {option}
              </button>
            ))}
          </div>
        </div>
      )}
    </div>
  );
};

export default Dropdown;
