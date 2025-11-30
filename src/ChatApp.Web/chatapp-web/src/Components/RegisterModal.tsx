import UsernameInput from "./UsernameInput.tsx";
import EmailInput from "./EmailInput.tsx";
import PasswordInput from "./PasswordInput.tsx";
import {useState} from "react";

interface RegisterRequest {
    username: string;
    email: string;
    password: string;
}

export default function RegisterModal() {
    const [form, setForm] = useState<RegisterRequest>({
        username: "",
        email: "",
        password: "",
    });

    const [errors, setErrors] = useState<string[]>([]);
    const [loading, setLoading] = useState(false);
    const [formSuccess, setFormSuccess] = useState(false);
    const [registeredEmail, setRegisteredEmail] = useState<string>("");
    const [emailResent, setEmailResent] = useState(false);

    const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        setForm({ ...form, [e.target.name]: e.target.value });
    };

    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault();
        setErrors([]);
        setLoading(true);

        try {
            const registerRes = await fetch("https://localhost:7073/api/user/register", {
                method: "POST",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify(form),
                credentials: "include"
            });

            if (!registerRes.ok) {
                const data = await registerRes.json();
                // flatten any validation errors returned by API and cast as string.
                const flattenedErrors = Object.values(data.errors).flat().map(e => String(e));
                setErrors(flattenedErrors);
            } else {
                // clear form and show success msg
                setRegisteredEmail(form.email);
                setForm({username: "", email: "", password: "", });
                setFormSuccess(true);
            }
        } catch {
            setErrors(["Something went wrong"]);
        } finally {
            setLoading(false);
        }
    };

    const resendEmail = async () => {
        setLoading(true);
        try {
            const res = await fetch('https://localhost:7073/api/User/resend-confirmation', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify({
                    email: registeredEmail
                })
            });

            if (!res.ok) {
                const data = await res.json();
                setErrors([data.message]);
            } else {
                setEmailResent(true);
            }
        } catch {
            setErrors(["Something went wrong"]);
        } finally {
            setLoading(false);
        }
    }

    return (
        <>
            {/* You can open the modal using document.getElementById('ID').showModal() method */}
            <button
                className="btn"
                onClick={() => (document.getElementById('my_modal_3') as HTMLDialogElement)?.showModal()}
            >
                Sign Up
            </button>

            <dialog id="my_modal_3" className="modal">
                <div className="modal-box">
                    <form method="dialog">
                        {/* if there is a button in form, it will close the modal */}
                        <button className="btn btn-sm btn-circle btn-ghost absolute right-2 top-2">✕</button>
                    </form>

                    <div className="flex flex-col gap-4">
                        <h2 className="font-bold text-xl">Sign Up!</h2>

                        {/* When form submits with a 200 response, hide it and show confirmation text. */}
                        {!formSuccess ? (
                            <form onSubmit={handleSubmit} className="gap-2">
                                <UsernameInput value={form.username} name={"username"} onChange={handleChange} />
                                <EmailInput value={form.email} name={"email"} onChange={handleChange} />
                                <PasswordInput value={form.password} name={"password"} onChange={handleChange} />
                                <button className="btn btn-soft btn-primary mt-4" type="submit" disabled={formSuccess}>
                                    {loading && (
                                        <span className="loading loading-spinner"></span>
                                    )}
                                    Register
                                </button>
                            </form>
                        ) : (
                            <div className="p-4 bg-green-100 border border-green-300 rounded-md text-green-800">
                                <p>
                                    Registration successful! We’ve sent a confirmation email to <strong>{registeredEmail}</strong>.
                                    Please check your inbox and click the link to activate your account.
                                </p>
                                <p className="mt-2">
                                    Didn’t receive the email?{' '}
                                    <span
                                        onClick={resendEmail}
                                        className="text-blue-600 hover:underline cursor-pointer"
                                    >
                                        Resend confirmation email
                                    </span>
                                </p>
                                {emailResent && (
                                    <p className="mt-2">Confirmation email resent!</p>
                                )}
                                {loading && (
                                    <p className="mt-2">Loading...</p>
                                )}
                            </div>
                        )}

                        {errors.length > 0 && (
                            <ul className={"text-red-700"}>
                                {errors.map((err, i) => <li key={i}>{err}</li>)}
                            </ul>
                        )}
                    </div>
                </div>
            </dialog>
        </>
    )
}