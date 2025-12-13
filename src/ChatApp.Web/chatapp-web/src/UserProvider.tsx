import {type ReactNode, useState} from "react";
import {type User, UserContext} from "./UserContext";

export const UserProvider = ({ children }: {children: ReactNode}) => {
    // storing state inside provider
    const [user, setUser] = useState<User>({
        username: "",
        email: "",
        isLoggedIn: false
    });

    return (
        // make user state accessible via context's provider
        <UserContext.Provider value={{ user, setUser}}>
            {children}
        </UserContext.Provider>
    );
};