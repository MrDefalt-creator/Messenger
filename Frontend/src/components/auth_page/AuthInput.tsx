import * as React from "react";

export default function AuthInput({children}: {children: React.ReactNode}) {
    return(
        <div className='flex relative w-full'>
            <span className='absolute top-2 left-3.5 text-base font-sans px-1
            bg-white pointer-events-none'>
                {children}
            </span>
            <input
                className='w-full p-2 border-2 rounded-xl border-teal-500
                font-sans z-10
                focus:outline-none focus:border-teal-600 focus:ease-in-out'/>
        </div>
    )
}