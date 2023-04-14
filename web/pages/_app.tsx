import "@/styles/globals.css";
import type { AppProps } from "next/app";
import "antd/dist/reset.css";
import React from "react";
import { Layout } from "antd";
import {
  Logo,
  Copyright,
  Api,
  GitHub,
  Twitter,
  Donate,
  Menu,
} from "@/src/components";
import styles from "./_app.module.css";

const { Header, Content, Footer } = Layout;

const App = ({ Component, pageProps }: AppProps) => (
  <Layout>
    <Header className={styles.header}>
      <Logo />
      <Menu />
    </Header>
    <Content className={styles.content}>
      <Component {...pageProps} />
    </Content>
    <Footer className={styles.footer}>
      <Copyright />
      <Api />
      <GitHub />
      <Twitter />
      <Donate />
    </Footer>
  </Layout>
);
export default App;
