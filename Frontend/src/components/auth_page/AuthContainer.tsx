import * as React from "react";

export default function AuthContainer({children}: {children: React.ReactNode}) {
    return(
        <section className='flex items-center flex-col relative max-w-[720px] mx-auto'>
            <div className='mx-auto w-[10rem] h-[10rem] mb-6'>
                <img src='/logo.svg' alt='Logo' className='w-full h-full'/>
            </div>
            <h4 className='font-sans text-3xl font-medium mt-6 mb-3.5'>
                Воспользуйтесь ChatMe
            </h4>
            <p className='font-sans text-slate-400 text-base font-normal'>
                Введите ваши данные для работы в ChatMe
            </p>
            <div className="mt-[49px] items-center w-full grid grid-cols-1 gap-6">
                {children}
            </div>
        </section>
    )
}