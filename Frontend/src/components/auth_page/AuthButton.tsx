import {MouseEventHandler} from "react";

interface AuthButtonProps {
    name: string;
    onClick?: MouseEventHandler<HTMLButtonElement>;
}
export default function AuthButton({name, onClick}: AuthButtonProps) {
    return(
        <button className='w-full bg-emerald-500 hover:bg-emerald-600
        p-3 rounded-xl transition-colors duration-200'
        onSubmit={onClick}>
            {name}
        </button>
    )
}