import { Space, Table } from "antd";
import { useListsTable } from "./useListsTable";
import { OData, FilterList } from "@/src/interfaces";
import { localeCompare } from "@/src/utils";
import { ShowListButton } from "./ShowListButton";

export const ListsTable = (props: OData<FilterList>) => {
  const listsTableProps = useListsTable(props);
  return (
    <Table<FilterList> {...listsTableProps}>
      {nameColumn}
      {descriptionColumn}
      {actionsColumn}
    </Table>
  );
};

const nameColumn = (
  <Table.Column<FilterList>
    dataIndex="name"
    title="Name"
    sorter={{
      compare: (a, b) => localeCompare({ a: a.name, b: b.name }),
      multiple: 1,
    }}
  />
);

const descriptionColumn = (
  <Table.Column<FilterList>
    dataIndex="description"
    title="Description"
    sorter={{
      compare: (a, b) => localeCompare({ a: a.description, b: b.description }),
      multiple: 2,
    }}
  />
);

const actionsColumn = (
  <Table.Column<FilterList>
    dataIndex="id"
    title="Actions"
    render={(id, filterlist) => (
      <Space>
        <ShowListButton listId={id} listName={filterlist.name} />
      </Space>
    )}
  />
);
