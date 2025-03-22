import { useState, ChangeEvent } from "react";

interface AuthInputProps  {
    placeholder: string;
    type?: string;
    onChange?: (e :ChangeEvent<HTMLInputElement>) => void;
    value?: string;
};

export default function AuthInput({ placeholder, type = "text",
                                  onChange, value}: AuthInputProps) {
    const [showPassword, setShowPassword] = useState(false);

    const toggleShowPassword = () => setShowPassword(prev => !prev);

    return (
        <div className="flex relative w-full">
            <input
                type={type === "password" && showPassword ? "text" : type}
                placeholder={placeholder}
                value={value}
                onChange={onChange}
                className="w-full p-3 pr-10 border-2 border-gray-400 rounded-xl font-sans outline-none focus:border-emerald-500 transition-colors duration-200 hover:border-[#00bc7d]"
            />
            {type === "password" && (
                <button
                    type="button"
                    onClick={toggleShowPassword}
                    className="absolute right-3 top-1/2 transform -translate-y-1/2 text-gray-500"
                >
                    {showPassword ? <img src='/open-eye.svg'
                                         alt='hide-psw'
                                         className='w-[25px] h-[32px]'
                    /> : <img src='/closed-eye.svg'
                              alt='show-psw'
                              className='w-[25px] h-[32px]'/>}
                </button>
            )}
        </div>
    );
}
