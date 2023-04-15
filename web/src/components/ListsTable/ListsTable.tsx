import { Space, Table } from "antd";
import { useListsTable } from "./useListsTable";
import { OData, FilterList, License } from "@/src/interfaces";
import { LicenseTag, ShowListButton } from "./components";

interface Props {
  lists: OData<FilterList>;
  licenses: License[];
}

export const ListsTable = ({ lists, licenses }: Props) => (
  <Table<FilterList> {...useListsTable({ ...lists })}>
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
      render={(id) => {
        const license = licenses.find((l) => l.id === id);
        return license && <LicenseTag {...license} />;
      }}
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
