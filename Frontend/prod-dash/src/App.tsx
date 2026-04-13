import './App.css'
import { useState } from "react"
import { Squares2X2Icon, ChartBarIcon, PlusIcon } from '@heroicons/react/24/outline'

type Task = {
  id: number
  title: string
  description: string
  status: "To Do" | "In Progress" | "Completed"
  createdAt: string
  dueDate: string
  completedAt?: string | null
  isActive: boolean
  hoursWorked: number
}

function App() {
  const [tasks, setTasks] = useState<Task[]>([
    {
      id: 11,
      title: "Fix authentication bug",
      description: "Users reporting issues with password reset flow",
      status: "To Do",
      createdAt: "2026-04-11T12:00:00Z",
      dueDate: "2026-04-18T12:00:00Z",
      completedAt: null,
      isActive: true,
      hoursWorked: 0,
    },
  ])

  const [selectedTask, setSelectedTask] = useState<Task | null>(null)
  const [isNewTask, setIsNewTask] = useState(false)

  const updateTaskStatus = (id: number, newStatus: Task["status"]) => {
    setTasks(prev =>
      prev.map(t =>
        t.id === id
          ? {
              ...t,
              status: newStatus,
              completedAt: newStatus === "Completed" ? new Date().toISOString() : t.completedAt,
            }
          : t
      )
    )
  }

  const addTask = (task: Task) => {
    setTasks(prev => [...prev, task])
  }

  return (
    <div className="min-h-screen bg-gray-100 flex flex-col">
      {/* Top Navigation */}
      <header className="bg-white shadow px-6 py-4 flex justify-between items-center">
        <div className="flex items-center gap-6">
          <h1 className="text-xl font-bold text-purple-600">ProdDash</h1>
          <nav className="flex gap-6 text-gray-600">
            <a href="#" className="flex items-center gap-2 hover:text-purple-600">
              <Squares2X2Icon className="h-5 w-5" />
              Tasks
            </a>
            <a href="#" className="flex items-center gap-2 hover:text-purple-600">
              <ChartBarIcon className="h-5 w-5" />
              Analytics
            </a>
          </nav>
        </div>
        <div className="flex items-center gap-4">
          <button
            className="flex items-center gap-2 bg-green-600 text-white px-3 py-1 rounded hover:bg-green-700"
            onClick={() => {
              setIsNewTask(true)
              setSelectedTask({
                id: Date.now(),
                title: "",
                description: "",
                status: "To Do",
                createdAt: new Date().toISOString(),
                dueDate: new Date().toISOString(),
                completedAt: null,
                isActive: true,
                hoursWorked: 0,
              })
            }}
          >
            <PlusIcon className="h-5 w-5" />
            New Task
          </button>
          <button className="text-sm text-red-500 hover:underline">Logout</button>
        </div>
      </header>

      {/* Board */}
      <main className="flex-1 p-6 grid grid-cols-3 gap-6">
        {["To Do", "In Progress", "Completed"].map(col => (
          <section key={col} className="bg-white rounded shadow p-4">
            <h2 className="text-lg font-semibold mb-4">{col}</h2>
            <div className="space-y-4">
              {tasks.filter(t => t.status === col).map(t => (
                <TaskCard key={t.id} task={t} onClick={setSelectedTask} />
              ))}
            </div>
          </section>
        ))}
      </main>

      {selectedTask && (
        <TaskModal
          task={selectedTask}
          onClose={() => {
            setSelectedTask(null)
            setIsNewTask(false)
          }}
          onStatusChange={updateTaskStatus}
          onSave={task => {
            if (isNewTask) {
              addTask(task)
            }
            setSelectedTask(null)
            setIsNewTask(false)
          }}
          isNew={isNewTask}
        />
      )}
    </div>
  )
}

function TaskCard({ task, onClick }: { task: Task; onClick: (t: Task) => void }) {
  return (
    <div
      className="border rounded p-3 shadow-sm hover:shadow-md cursor-pointer"
      onClick={() => onClick(task)}
    >
      <h3 className="font-semibold text-gray-800">{task.title}</h3>
      <p className="text-sm text-gray-600">{task.description}</p>
      <div className="flex justify-between text-xs text-gray-500 mt-2">
        <span>📅 {new Date(task.dueDate).toLocaleDateString()}</span>
        {task.hoursWorked > 0 && <span>⏱ {task.hoursWorked}h</span>}
      </div>
    </div>
  )
}

function TaskModal({
  task,
  onClose,
  onStatusChange,
  onSave,
  isNew,
}: {
  task: Task
  onClose: () => void
  onStatusChange: (id: number, newStatus: Task["status"]) => void
  onSave: (task: Task) => void
  isNew: boolean
}) {
  const [localTask, setLocalTask] = useState<Task>(task)

  return (
    <div className="fixed inset-0 bg-black bg-opacity-50 flex items-center justify-center">
      <div className="bg-white rounded-lg shadow-lg p-6 w-[400px]">
        <h2 className="text-xl font-bold mb-4">{isNew ? "New Task" : "Edit Task"}</h2>
        <form
          className="space-y-4"
          onSubmit={e => {
            e.preventDefault()
            onSave(localTask)
          }}
        >
          <div>
            <label className="block text-sm font-medium">Title</label>
            <input
              type="text"
              value={localTask.title}
              onChange={e => setLocalTask({ ...localTask, title: e.target.value })}
              className="w-full border rounded px-2 py-1"
            />
          </div>
          <div>
            <label className="block text-sm font-medium">Description</label>
            <textarea
              value={localTask.description}
              onChange={e => setLocalTask({ ...localTask, description: e.target.value })}
              className="w-full border rounded px-2 py-1"
            />
          </div>
          <div>
            <label className="block text-sm font-medium">Status</label>
            <select
              value={localTask.status}
              onChange={e => {
                const newStatus = e.target.value as Task["status"]
                setLocalTask({ ...localTask, status: newStatus })
                onStatusChange(localTask.id, newStatus)
              }}
              className="w-full border rounded px-2 py-1"
            >
              <option>To Do</option>
              <option>In Progress</option>
              <option>Completed</option>
            </select>
          </div>
          <div>
            <label className="block text-sm font-medium">Due Date</label>
            <input
              type="date"
              value={localTask.dueDate.split("T")[0]}
              onChange={e => setLocalTask({ ...localTask, dueDate: e.target.value })}
              className="w-full border rounded px-2 py-1"
            />
          </div>
          <div>
            <label className="block text-sm font-medium">Hours Worked</label>
            <input
              type="number"
              value={localTask.hoursWorked}
              onChange={e => setLocalTask({ ...localTask, hoursWorked: Number(e.target.value) })}
              className="w-full border rounded px-2 py-1"
            />
          </div>
          <div className="flex justify-end gap-2">
            <button type="button" onClick={onClose} className="px-3 py-1 border rounded">
              Cancel
            </button>
            <button type="submit" className="px-3 py-1 bg-purple-600 text-white rounded">
              {isNew ? "Add Task" : "Save Changes"}
            </button>
          </div>
        </form>
      </div>
    </div>
  )
}

export default App