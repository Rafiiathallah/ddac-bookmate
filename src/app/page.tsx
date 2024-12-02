"use client";

import { motion } from "framer-motion";
import { Book, Library, Search, Cloud, Star } from "lucide-react";
import Link from "next/link";

// --------------------------------
// Constants
// --------------------------------
const features = [
  {
    icon: Library,
    title: "Vast Collection",
    description:
      "Access thousands of books across all genres, from classics to latest releases",
  },
  {
    icon: Search,
    title: "Smart Discovery",
    description:
      "Find your next favorite book with personalized recommendations",
  },
  {
    icon: Cloud,
    title: "Cloud Library",
    description: "Access your books anywhere, anytime, across all your devices",
  },
  {
    icon: Star,
    title: "Collections",
    description: "Create custom collections and share them with the community",
  },
];

// Simplified book positions
const BOOK_POSITIONS = [
  { top: 20, left: 10, rotate: 45 },
  { top: 70, left: 80, rotate: 90 },
  { top: 40, left: 30, rotate: 180 },
  { top: 80, left: 50, rotate: 120 },
  { top: 30, left: 70, rotate: 200 },
  { top: 60, left: 20, rotate: 150 },
  { top: 50, left: 90, rotate: 30 },
  { top: 10, left: 40, rotate: 220 },
];

// --------------------------------
// Main Component
// --------------------------------
export default function Home() {
  return (
    <div className="min-h-screen bg-gradient-to-br from-blue-50 to-indigo-100">
      {/* Animated Background */}
      <div className="absolute inset-0 overflow-hidden">
        {BOOK_POSITIONS.map((position, i) => (
          <motion.div
            key={i}
            className="absolute opacity-10"
            initial={{
              top: `${position.top}%`,
              left: `${position.left}%`,
              rotate: position.rotate,
            }}
            animate={{
              top: `${(position.top + 20) % 100}%`,
              left: `${(position.left + 20) % 100}%`,
              rotate: position.rotate + 180,
            }}
            transition={{
              duration: 15 + (i % 5), // Simplified timing
              repeat: Infinity,
              repeatType: "reverse",
            }}
          >
            <Book className="h-16 w-16 text-blue-600" />
          </motion.div>
        ))}
      </div>

      <div className="relative">
        {/* Updated Navigation - moved to left */}
        <nav className="mx-auto max-w-7xl px-6 py-6 lg:px-8">
          <div className="flex items-center">
            <Book className="h-8 w-8 text-blue-600" />
            <span className="text-xl font-semibold text-blue-600 ml-2">
              Bookmate
            </span>
          </div>
        </nav>

        <main className="relative mx-auto max-w-7xl px-6 sm:py-40 lg:px-8">
          {/* Hero Section - adjusted spacing */}
          <motion.div
            initial={{ opacity: 0, y: 20 }}
            animate={{ opacity: 1, y: 0 }}
            transition={{ duration: 0.8 }}
            className="space-y-12" // Increased space between elements
          >
            <div className="space-y-8 text-center">
              <h1 className="text-4xl font-bold tracking-tight text-gray-900 sm:text-6xl">
                Your Digital Library Awaits
              </h1>
              <p className="mx-auto max-w-2xl text-lg text-gray-600">
                Discover, read, and collect your favorite books in one place.
                Join thousands of readers in their digital reading journey.
              </p>
            </div>
            <div className="flex justify-center">
              <Link href="/explore">
                <motion.div
                  whileHover={{ scale: 1.05 }}
                  whileTap={{ scale: 0.95 }}
                >
                  <button className="rounded-full bg-blue-600 px-8 py-3 text-lg font-semibold text-white shadow-lg transition-colors hover:bg-blue-700">
                    Explore Library
                  </button>
                </motion.div>
              </Link>
            </div>
          </motion.div>

          {/* Features Section */}
          <motion.div
            initial={{ opacity: 0, y: 40 }}
            animate={{ opacity: 1, y: 0 }}
            transition={{ duration: 0.8, delay: 0.2 }}
            className="mt-32"
          >
            <h2 className="text-3xl font-bold text-gray-900 mb-16">
              Why Bookmate?
            </h2>
            <div className="relative">
              <div className="absolute inset-0 flex items-center justify-center">
                <div className="h-96 w-96 rounded-full bg-blue-200 opacity-50 blur-3xl"></div>
              </div>
              <motion.div
                className="relative grid grid-cols-1 gap-8 sm:grid-cols-2"
                initial="hidden"
                animate="visible"
                variants={{
                  hidden: { opacity: 0 },
                  visible: {
                    opacity: 1,
                    transition: {
                      delayChildren: 0.3,
                      staggerChildren: 0.2,
                    },
                  },
                }}
              >
                {features.map((feature, index) => (
                  <motion.div
                    key={feature.title}
                    className="group"
                    variants={{
                      hidden: { y: 20, opacity: 0 },
                      visible: { y: 0, opacity: 1 },
                    }}
                  >
                    <div className="relative overflow-hidden rounded-lg bg-white p-6 shadow-lg transition-all hover:shadow-xl">
                      <div className="absolute inset-0 bg-gradient-to-r from-blue-600 to-indigo-600 opacity-0 transition-opacity group-hover:opacity-10" />
                      <div className="relative flex items-center">
                        <feature.icon className="h-12 w-12 text-blue-600 mr-4" />
                        <div className="text-left">
                          <h3 className="text-lg font-semibold text-gray-900">
                            {feature.title}
                          </h3>
                          <p className="mt-2 text-sm text-gray-600">
                            {feature.description}
                          </p>
                        </div>
                      </div>
                    </div>
                  </motion.div>
                ))}
              </motion.div>
            </div>
          </motion.div>
        </main>

        {/* Footer */}
        <footer className="mt-20 bg-white bg-opacity-80 py-8">
          <div className="mx-auto max-w-7xl px-6 lg:px-8">
            <div className="flex flex-col items-center justify-between sm:flex-row">
              <p className="text-sm text-gray-500">
                © {new Date().getFullYear()} Bookmate. All rights reserved.
              </p>
              <div className="mt-4 flex space-x-6 sm:mt-0">
                <a
                  href="#"
                  className="text-gray-400 hover:text-blue-600 transition-colors duration-200"
                >
                  Privacy Policy
                </a>
                <a
                  href="#"
                  className="text-gray-400 hover:text-blue-600 transition-colors duration-200"
                >
                  Terms of Service
                </a>
                <a
                  href="#"
                  className="text-gray-400 hover:text-blue-600 transition-colors duration-200"
                >
                  Contact Us
                </a>
              </div>
            </div>
          </div>
        </footer>
      </div>
    </div>
  );
}
