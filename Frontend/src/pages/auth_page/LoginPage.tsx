import AuthPlaceholder from "../../components/auth_page/AuthPlaceholder.tsx";
import AuthContainer from "../../components/auth_page/AuthContainer.tsx";

export default function LoginPage(){
    return (
        <main className='flex items-center flex-col relative max-w-[720px] mx-auto'>
            <AuthPlaceholder/>
                <AuthContainer>

                </AuthContainer>
            <AuthPlaceholder/>
        </main>
    )
}