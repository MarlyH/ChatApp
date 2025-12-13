import React, {createContext} from 'react';

export interface User {
    username: string;
    email: string;
    isLoggedIn: boolean;
}

export const UserContext = createContext<{
    user: User;
    setUser: React.Dispatch<React.SetStateAction<User>>
} | null>(null);



