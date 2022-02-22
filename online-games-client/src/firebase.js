// Import the functions you need from the SDKs you need
import { initializeApp } from "firebase/app";
import { getAuth, GoogleAuthProvider } from "firebase/auth";
// TODO: Add SDKs for Firebase products that you want to use
// https://firebase.google.com/docs/web/setup#available-libraries

// Your web app's Firebase configuration
// For Firebase JS SDK v7.20.0 and later, measurementId is optional
const firebaseConfig = {
  apiKey: "AIzaSyAsFqv-h_87NPXt-Z8O_T9IxL6U-diJD0U",
  authDomain: "fir-auth-202b2.firebaseapp.com",
  projectId: "fir-auth-202b2",
  storageBucket: "fir-auth-202b2.appspot.com",
  messagingSenderId: "525482780489",
  appId: "1:525482780489:web:973623607c6f92e48a5c98",
  measurementId: "G-R0NEFFG4V9"
};

// Initialize Firebase
export const app = initializeApp(firebaseConfig);
export const auth = getAuth();
export const provider = new GoogleAuthProvider();