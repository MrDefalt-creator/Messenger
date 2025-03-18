import AuthContainer from "../../components/auth_page/AuthContainer.tsx";
import AuthInput from "../../components/auth_page/AuthInput.tsx";

export default function LoginPage(){
    return (
        <main className='block my-[15rem]'>
            <div className='flex items-center flex-col relative max-w-[720px] mx-auto'>
                <AuthContainer>
                    <AuthInput>Email</AuthInput>
                </AuthContainer>
            </div>
        </main>
    )
}