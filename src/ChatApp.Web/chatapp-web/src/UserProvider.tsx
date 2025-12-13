import {type ReactNode, useEffect, useState} from "react";
import {type User, UserContext} from "./UserContext";

export const UserProvider = ({ children }: {children: ReactNode}) => {
    // storing state inside provider
    const [user, setUser] = useState<User>({
        username: "",
        email: "",
        isLoggedIn: false
    });

    useEffect(() => {
        console.log("User state in provider updated:\n" + JSON.stringify(user));
    }, [user]);

    // when the provider mounts on startup, check if the user is logged in.
    // if user is logged in, we fetch information about their profile.
    useEffect(() => {
        const fetchProfile = async () => {
            try {
                const getProfileRes = await fetch("https://localhost:7073/api/user/me", {
                    credentials: "include"
                });

                if (getProfileRes.status === 401) {
                    setUser({username: "", email: "", isLoggedIn: false});
                }

                if (getProfileRes.ok) {
                    const data = await getProfileRes.json();
                    setUser({
                        username: data.username,
                        email: data.email,
                        isLoggedIn: true
                    });
                }

            } catch {
                setUser({username: "", email: "", isLoggedIn: false});
            }
        }

        void fetchProfile();
    }, [])

    return (
        // make user state accessible via context's provider
        <UserContext.Provider value={{ user, setUser}}>
            {children}
        </UserContext.Provider>
    );
};