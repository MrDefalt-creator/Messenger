import * as React from "react";

export default function AuthInput({children}: {children: React.ReactNode}) {
    return(
        <div className='flex relative w-full'>
            <span className='absolute top-3 left-3.5 text-base font-sans px-1
            bg-white pointer-events-none'>
                {children}
            </span>
            <input
                className='auth-input'
                />
        </div>
    )
}