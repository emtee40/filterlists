import { ListsTable } from "@/src/components";
import { InferGetStaticPropsType } from "next";

import Head from "next/head";

export default function Home({
  filterlists,
}: InferGetStaticPropsType<typeof getStaticProps>) {
  return (
    <>
      <Head>
        <title>
          FilterLists | Subscriptions for uBlock Origin, Adblock Plus,
          AdGuard,...
        </title>
        <meta
          name="description"
          content="FilterLists is the independent, comprehensive directory of filter and host lists for advertisements, trackers, malware, and annoyances. By Collin M. Barrett."
        />
        <meta name="viewport" content="width=device-width, initial-scale=1" />
        <link rel="icon" href="/favicon.ico" />
      </Head>
      <main>
        <ListsTable {...filterlists} />
      </main>
    </>
  );
}

export async function getStaticProps() {
  const res = await fetch(
    process.env.NEXT_PUBLIC_FILTERLISTS_API_URL + "/lists?$count=true&$top=10"
  );
  const filterlists = await res.json();
  return {
    props: { filterlists },
  };
}
