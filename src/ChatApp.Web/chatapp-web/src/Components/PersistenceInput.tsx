import React from "react";

interface PersistenceInputProps {
    name: string,
    checked: boolean,
    handleChange: (e: React.ChangeEvent<HTMLInputElement>) => void
}

export default function PersistenceInput({name, checked, handleChange}: PersistenceInputProps) {
    return (
        <div>
            <label>
                Stay logged in?
                <input
                    className="m-2"
                    type={"checkbox"}
                    name={name}
                    checked={checked}
                    onChange={handleChange} />
            </label>
        </div>
    )
}