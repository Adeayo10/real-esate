import React, { useState } from "react";
import "./Auth.css";
localStorage.setItem("loggedInUser", JSON.stringify({ firstName: "Javeed" })); 
localStorage.setItem("isLoggedIn", true);


const Login = () => {
  const [loginData, setLoginData] = useState({
    email: "",
    password: ""
  });

  // Dummy user data for login
  const dummyUser = {
    email: "testuser@example.com",
    password: "123456"
  };

  const handleChange = (e) => {
    const { name, value } = e.target;
    setLoginData((prev) => ({
      ...prev,
      [name]: value
    }));
  };

  const handleSubmit = (e) => {
    e.preventDefault();

    if (
      loginData.email === dummyUser.email &&
      loginData.password === dummyUser.password
    ) {
      alert("Login successful!");
      localStorage.setItem("isLoggedIn", true);
      window.location.href = "/"; 
    } else {
      alert("Invalid email or password");
    }
  };

  return (
    <div className="auth-wrapper">
      <div className="auth-container">
        <h2>Login</h2>
        <form className="auth-form" onSubmit={handleSubmit}>
          <label>Email</label>
          <input
            type="email"
            name="email"
            placeholder="Enter your email"
            value={loginData.email}
            onChange={handleChange}
            required
          />

          <label>Password</label>
          <input
            type="password"
            name="password"
            placeholder="Enter your password"
            value={loginData.password}
            onChange={handleChange}
            required
          />

          <button type="submit">Login</button>

          <div className="auth-toggle">
            Don’t have an account? <a href="/signup">Sign Up</a>
          </div>
        </form>
      </div>
    </div>
  );
};

export default Login;
