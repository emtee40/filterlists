import { Space, Table } from "antd";
import { useListsTable } from "./useListsTable";
import { FilterList, ListTable } from "@/src/interfaces";
import {
  LanguageCloud,
  LicenseTag,
  ShowListButton,
  TagCloud,
} from "./components";

export const ListsTable = ({
  filterLists,
  languages,
  licenses,
  tags,
}: ListTable) => (
  <Table<FilterList> {...useListsTable({ ...filterLists })}>
    <Table.Column<FilterList>
      dataIndex="name"
      title="Name"
      sorter={{
        multiple: 1,
      }}
    />
    <Table.Column<FilterList>
      dataIndex="description"
      title="Description"
      sorter={{
        multiple: 3,
      }}
    />
    <Table.Column<FilterList>
      dataIndex="licenseId"
      title="License"
      sorter={{
        multiple: 2,
      }}
      render={(licenseId) => {
        const license = licenses.find((l) => l.id === licenseId);
        return license && <LicenseTag {...license} />;
      }}
    />
    <Table.Column<FilterList>
      dataIndex="languageIds"
      title="Languages"
      render={(languageIds: number[]) => (
        <LanguageCloud
          languages={languages.filter((l) => languageIds.includes(l.id))}
        />
      )}
    />
    <Table.Column<FilterList>
      dataIndex="tagIds"
      title="Tags"
      render={(tagIds: number[]) => (
        <TagCloud tags={tags.filter((t) => tagIds.includes(t.id))} />
      )}
    />
    <Table.Column<FilterList>
      dataIndex="id"
      title="Actions"
      render={(id, filterlist) => (
        <Space>
          <ShowListButton listId={id} listName={filterlist.name} />
        </Space>
      )}
    />
  </Table>
);
