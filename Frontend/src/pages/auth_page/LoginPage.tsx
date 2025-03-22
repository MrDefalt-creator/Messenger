import AuthContainer from "../../components/auth_page/AuthContainer.tsx";
import AuthInput from "../../components/auth_page/AuthInput.tsx";
import AuthButton from "../../components/auth_page/AuthButton.tsx";
import {useState, ChangeEvent } from "react";


export default function LoginPage(){
    const handleSetEmail
        = (e:ChangeEvent<HTMLInputElement>) => {
        setEmail(e.target.value);
    }
    const handleSetPassword
        = (e:ChangeEvent<HTMLInputElement>) => {
        setPassword(e.target.value);
    }

    const [email, setEmail] = useState("");
    const [password, setPassword] = useState("");
    return (
        <main className='block my-[15rem]'>
            <div className='flex items-center flex-col relative max-w-[720px] mx-auto'>
                <AuthContainer>
                    <AuthInput placeholder={"Email"} value={email}
                               onChange={handleSetEmail}/>
                    <AuthInput placeholder={"Password"} type={"password"}
                    value={password} onChange={handleSetPassword}/>
                    <AuthButton name={"Войти"}/>
                </AuthContainer>
            </div>
        </main>
    )
}